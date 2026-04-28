# 2D Vertical Space Shooter

<p align="center">
  <img src="Assets/Vertical%202D%20Shooting%20BE4/App%20Icon.png" width="120" alt="2D Vertical Space Shooter icon">
</p>

<p align="center">
  Unity 6로 제작한 2D 세로형 슈팅 게임입니다.<br>
  적 웨이브를 피하고, 아이템으로 무장을 강화하고, Boom으로 화면을 정리하며 점수를 올리는 구조입니다.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Unity-6000.4.1f1-black?logo=unity" alt="Unity version">
  <img src="https://img.shields.io/badge/Genre-Vertical%20Shooter-0f766e" alt="Genre">
  <img src="https://img.shields.io/badge/Platform-PC%20Play%20Mode-2563eb" alt="Platform">
  <img src="https://img.shields.io/badge/Language-C%23-7c3aed" alt="Language">
</p>

---

## At a Glance

| 구분 | 내용 |
| --- | --- |
| 한 줄 설명 | 위에서 내려오는 적을 격파하며 생존 점수를 쌓는 종스크롤 슈팅 게임 |
| 핵심 재미 | 회피, 연사, 파워업, 아이템 수집, Boom을 활용한 긴급 화면 정리 |
| 현재 구현 | 플레이어 이동, 연속 발사, CSV 웨이브 스폰, 아이템 드롭, 파워 강화, Boom 시스템, 오브젝트 풀링, 게임오버 UI, 배경 스크롤 |
| 플레이 목표 | 최대한 오래 살아남아 높은 점수를 기록하는 것 |

---

## What You See In Game

| 요소 | 플레이 경험 |
| --- | --- |
| Player | 좌우/상하 이동으로 탄막과 적을 회피 |
| Shot | 좌클릭 홀드로 지속 발사 |
| Power | 화력이 단계적으로 상승 |
| Boom | 우클릭 한 번으로 적과 적 탄환을 빠르게 정리 |
| Coin | 점수 확보용 보상 |
| Life UI | 남은 생존 기회를 직관적으로 표시 |
| Boom UI | 현재 보유한 Boom 수량 표시 |
| Scrolling Background | 화면이 계속 진행되는 느낌을 주는 자동 배경 스크롤 |

---

## Gameplay Flow

`Move` → `Shoot` → `Defeat Enemies` → `Collect Items` → `Power Up or Store Boom` → `Use Boom in Danger` → `Survive Longer` → `Reach Higher Score`

---

## Controls

| 입력 | 동작 |
| --- | --- |
| WASD / 방향키 | 플레이어 이동 |
| 마우스 좌클릭 홀드 | 연속 발사 |
| 마우스 우클릭 | Boom 사용 |

---

## Core Systems

### 1. CSV 기반 웨이브 스폰

코드 수정 없이 `Resources/StageData.csv` 파일만 편집해서 웨이브를 자유롭게 구성할 수 있습니다.

**CSV 형식** — `delay,type,point`

| 필드 | 설명 | 예시 |
| --- | --- | --- |
| delay | 이전 스폰 후 대기 시간(초) | `1`, `0.2` |
| type | 적 종류 (`S`=소형 / `A`=중형 / `B`=대형) | `S` |
| point | 스폰 포인트 번호 (0=좌측 끝 ~ 4=우측 끝) | `1`, `3` |

**예시 (`Assets/Resources/StageData.csv`)**

```
1,S,1
0.2,S,1
0.2,S,1
0.2,S,1
0.2,S,1
1,S,3
0.2,S,3
0.2,S,3
```

**구현 방식**

- `Resources.Load<TextAsset>("StageData")` 로 런타임에서 CSV를 불러옵니다.
- `StringReader`로 한 줄씩 읽고, `Split(',')` 으로 파싱합니다.
- 코루틴이 delay만큼 기다린 후 해당 타입·포인트에 적을 스폰합니다.
- 타입 → `ObjectPoolManager.enemyPrefabs` 배열 인덱스 매핑: `S`=0, `A`=1, `B`=2

---

### 2. Object Pooling

- 게임 시작 시 총알, 적, 아이템을 미리 생성해 비활성화 상태로 보관합니다.
- `ObjectPoolManager` 싱글톤이 모든 풀을 관리합니다.
- `Instantiate` / `Destroy` 대신 `SetActive(true/false)` 로 오브젝트를 재사용합니다.
- GC(Garbage Collection) 발생을 최소화해 런타임 성능을 개선합니다.

| 풀 대상 | 설명 |
| --- | --- |
| 플레이어 총알 (Power 1~3) | 발사 시 풀에서 꺼내고, 화면 밖 이탈 또는 적 충돌 시 반환 |
| 적 총알 | 적이 발사, 화면 밖 이탈 또는 플레이어 충돌 시 반환 |
| 적 | 스폰 시 풀에서 꺼내고, 사망 또는 화면 밖 이탈 시 반환 (재사용 시 HP 자동 리셋) |
| 아이템 (Coin / Power / Boom) | 드롭 시 풀에서 꺼내고, 수집 또는 화면 밖 이탈 시 반환 |

---

### 3. Shooting and Power Level

- 기본 공격은 정면 연사입니다.
- Power 아이템을 획득하면 무기 단계가 상승합니다.
- 최대 3단계까지 강화되며, 단계에 따라 다른 총알 풀을 사용합니다.

---

### 4. Boom System

- Boom 아이템을 획득하면 최대 3개까지 저장됩니다.
- 우클릭으로 즉시 발동할 수 있습니다.
- 발동 시 폭발 이펙트가 재생되며, 일정 시간 동안 적에게 큰 피해를 반복 적용합니다.
- 같은 시간 동안 화면의 적 탄환을 풀로 반환(제거)합니다.
- 사용 후 Boom UI가 즉시 갱신됩니다.

---

### 5. Item Drop System

적 처치 시 아이템이 확률적으로 생성됩니다.

| 드롭 결과 | 확률 | 효과 |
| --- | --- | --- |
| 없음 | 30% | 추가 보상 없음 |
| Coin | 30% | 점수 +100 |
| Power | 20% | 점수 +500, 화력 강화 |
| Boom | 20% | 점수 +200, Boom +1 |

---

### 6. UI Feedback

| UI | 설명 |
| --- | --- |
| Score | 현재 점수를 실시간으로 표시 |
| Life | 남은 라이프 3칸을 시각적으로 관리 |
| Boom | Boom 보유 개수를 별도 아이콘으로 표시 |
| Game Over | 최종 점수와 Retry 버튼 제공 |

---

### 7. Background Scrolling

- 3장의 배경 SpriteRenderer를 순환 배치해 자동 스크롤을 구현했습니다.
- 씬이 정지된 화면이 아니라 계속 전진하는 슈팅 게임처럼 느껴지도록 구성했습니다.

---

## Latest Updates

### 2026-04-28 — CSV 기반 웨이브 스폰 구현

- `Assets/Resources/StageData.csv` 신규 추가 — 코드 수정 없이 웨이브 패턴을 편집 가능
- `EnemyGenerate.cs` 전면 교체 — 랜덤 타이머 스폰 → CSV 순서 기반 코루틴 스폰
- `StringReader` + `Split(',')` 파싱 방식으로 요구사항 준수
- 타입 문자열(`S` / `A` / `B`) → `ObjectPoolManager.enemyPrefabs` 인덱스 매핑

---

### 2026-04-27 — 버그 수정 (발사·Boom·UI)

- `GameScene`에 `ObjectPoolManager` GameObject를 직접 추가 — 씬 실행 즉시 모든 풀 초기화
- 좌클릭 연속 발사, 우클릭 Boom 발동, 우상단 Boom UI 정상 작동
- `SpawnPoint` 에 `ObjectPoolManager.Instance` null 가드 추가 → NullReferenceException 해소

---

### 2026-04-27 — 오브젝트 풀링 도입

- `ObjectPoolManager` 싱글톤 신규 추가
- 플레이어 총알(Power 1~3), 적 총알, 적, 아이템 전체에 풀링 적용
- `Instantiate` / `Destroy` 호출 제거 → `SetActive` 기반 재사용으로 전환
- `EnemyController` `OnEnable` 에서 HP / 스프라이트 자동 리셋

---

### 이전 업데이트

- Boom 애니메이션과 Boom 전용 프리팹 추가
- Player, GameOver, ItemManager3 로직 확장
- 아이템 드롭을 확률형 구조로 변경
- BackgroundScroller 추가로 자동 배경 스크롤 구현

---

## Project Structure

```text
spaceShooters/
├── Assets/
│   ├── Resources/
│   │   └── StageData.csv          ← 웨이브 데이터 (CSV)
│   ├── Scenes/
│   │   └── GameScene.unity
│   ├── Scripts/
│   │   ├── EnemyGenerate.cs       ← CSV 웨이브 스폰
│   │   ├── ObjectPoolManager.cs   ← 오브젝트 풀 관리
│   │   ├── EnemyController.cs
│   │   ├── Player.cs
│   │   ├── GameOver.cs
│   │   ├── ItemManager3.cs
│   │   ├── BackgroundScroller.cs
│   │   ├── SpawnPoint.cs
│   │   └── ...
│   ├── Prefabs/
│   │   ├── Enemy A.prefab
│   │   ├── Enemy B.prefab
│   │   ├── Enemy C.prefab
│   │   └── ...
│   └── Animations/
├── Packages/
└── ProjectSettings/
```

---

## How To Run

**Requirements**: Unity 6000.4.1f1 이상

```bash
git clone https://github.com/Masil21/2d-vertical-shooter.git
```

1. Unity Hub에서 클론한 폴더를 엽니다.
2. `Assets/Scenes/GameScene.unity` 씬을 엽니다.
3. Inspector에서 `ObjectPoolManager`의 `Enemy Prefabs` 배열을 확인합니다.
   - `[0]` = Enemy A (소형), `[1]` = Enemy B (중형), `[2]` = Enemy C (대형)
4. Play로 실행합니다.

웨이브 패턴 변경 시 `Assets/Resources/StageData.csv` 를 편집하면 됩니다.

---

## Project Overview

| 항목 | 내용 |
| --- | --- |
| 프로젝트명 | 2d-vertical-shooter |
| 장르 | 2D Vertical Shooter |
| 엔진 | Unity 6000.4.1f1 |
| 렌더링 | Universal Render Pipeline |
| 언어 | C# |
