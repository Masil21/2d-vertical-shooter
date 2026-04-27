# 2D Vertical Space Shooter

Unity로 제작한 2D 세로형 슈팅 게임입니다.
적 웨이브를 피하고 격파하면서 점수를 올리고, 아이템으로 전투 성능을 강화할 수 있습니다.

## Project Overview

| 항목 | 내용 |
| --- | --- |
| 프로젝트명 | 2d-vertical-shooter |
| 장르 | 2D Vertical Shooter |
| 엔진 | Unity 6000.4.1f1 (Unity 6) |
| 렌더링 | URP (Universal Render Pipeline) |
| 언어 | C# |

## Recent Updates (2026-04-27)

이번 최신 반영에서 플레이 감각에 직접 영향을 주는 시스템이 추가/개선되었습니다.

- Boom(폭탄) 시스템 추가
- 우클릭으로 Boom 발동, 일정 시간 동안 화면 정리
- Boom 보유량 UI(최대 3개) 추가
- 아이템 드롭 로직 확률형으로 변경
- 플레이어/적/씬/프리팹/애니메이션 연동 업데이트

## Core Gameplay

### Controls

| 입력 | 동작 |
| --- | --- |
| WASD / 방향키 | 이동 |
| 마우스 좌클릭(홀드) | 연속 발사 |
| 마우스 우클릭 | Boom 사용 (보유 시) |

### Rules

- 플레이어는 화면 경계 내부에서만 이동합니다.
- 적 혹은 적 탄환과 충돌하면 목숨이 감소합니다.
- 목숨이 모두 소진되면 게임오버 패널이 표시됩니다.
- 점수는 실시간으로 UI에 반영됩니다.
- Retry 버튼으로 현재 씬을 즉시 재시작할 수 있습니다.

## Combat Systems

### Shot Power Level

- 기본 발사는 Power1 탄환입니다.
- Power 아이템 획득 시 최대 3단계까지 강화됩니다.
- 파워 단계에 따라 발사 프리팹이 변경됩니다.

### Boom System

- Boom 아이템 획득 시 보유량이 증가합니다. (최대 3)
- 우클릭으로 Boom을 사용하면:
- 화면 중앙에 폭발 이펙트가 재생됩니다.
- 일정 시간 동안 적에게 큰 피해를 반복 적용합니다.
- 화면의 적 탄환을 지속 제거합니다.
- 사용 즉시 Boom UI가 감소 반영됩니다.

## Item System

적 처치 시 아이템이 드롭될 수 있으며, 현재 확률은 다음과 같습니다.

| 결과 | 확률 |
| --- | --- |
| 드롭 없음 | 30% |
| Coin | 30% |
| Power | 20% |
| Boom | 20% |

아이템 효과:
- Coin: 점수 +100
- Power: 점수 +500, 파워 레벨 상승
- Boom: 점수 +200, Boom 보유량 증가

## UI Systems

- Score UI: 현재 점수 실시간 출력
- Life UI: 3칸 라이프 아이콘 관리
- Boom UI: Boom 보유 개수 시각화
- Game Over UI: 최종 점수 표시 및 Retry

## Project Structure

```text
spaceShooters/
├── Assets/
│   ├── Scenes/
│   │   └── GameScene.unity
│   ├── Scripts/
│   │   ├── Player.cs
│   │   ├── GameOver.cs
│   │   ├── ItemManager3.cs
│   │   ├── EnemyController.cs
│   │   ├── SpawnPoint.cs
│   │   └── ...
│   ├── Prefabs/
│   └── Animations/
├── Packages/
└── ProjectSettings/
```

## How To Run

### Requirements

- Unity 6000.4.1f1 이상

### Steps

1. 저장소 클론

```bash
git clone https://github.com/Masil21/2d-vertical-shooter.git
```

2. Unity Hub에서 프로젝트 폴더 열기
3. `Assets/Scenes/GameScene.unity` 열기
4. Play 실행

## Commit Summary (Latest)

- `848526f` Update changes
  - Boom 애니메이션/프리팹 추가
  - Player/GameOver/ItemManager3 로직 확장
  - GameScene 및 적/탄환 프리팹 동기화
- `903b5f3` Update scenes, scripts, animations, and prefabs
  - 아이템/이펙트 관련 에셋과 스크립트 대규모 업데이트
- `a750ee6` Restructure project assets and scripts
  - 스크립트/애셋 구조 정리
