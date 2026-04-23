# 🚀 2D Vertical Space Shooter

Unity로 제작한 2D 세로형 슈팅 게임입니다.
플레이어 전투기를 조작해 다양한 방향으로 진입하는 적을 격파하고 점수를 올리는 구조입니다.

---

## 📋 목차

- [프로젝트 개요](#-프로젝트-개요)
- [개발 환경](#-개발-환경)
- [게임플레이](#-게임플레이)
- [프로젝트 구조](#-프로젝트-구조)
- [스크립트 설명](#-스크립트-설명)
- [적 타입 & 점수 시스템](#-적-타입--점수-시스템)
- [플레이어 공격 시스템](#-플레이어-공격-시스템)
- [UI 시스템](#-ui-시스템)
- [설치 및 실행](#-설치-및-실행)

---

## 🎮 프로젝트 개요

| 항목 | 내용 |
|------|------|
| 프로젝트명 | 2d-vertical-shooter |
| 장르 | 2D 종스크롤 슈팅 (Vertical Shooter) |
| 엔진 | Unity 6000.4.1f1 (Unity 6) |
| 렌더 파이프라인 | Universal Render Pipeline (URP) |
| 개발 언어 | C# |
| 주요 특징 | 플레이어 이동/사격, 다중 스폰 포인트 적 생성, 점수/목숨/게임오버 UI |

---

## 🛠 개발 환경

### Unity 버전
- Unity 6000.4.1f1

### 주요 패키지
| 패키지 | 버전 | 용도 |
|--------|------|------|
| Universal Render Pipeline | 17.4.0 | 렌더링 |
| Input System | 1.19.0 | 입력 처리 |
| 2D Animation | 14.0.3 | 2D 애니메이션 |
| 2D Sprite | 1.0.0 | 스프라이트 렌더링 |
| TextMesh Pro | 내장 | 점수/게임오버 텍스트 UI |

### 권장 IDE
- JetBrains Rider
- Visual Studio
- VS Code

---

## 🕹 게임플레이

### 조작 방법
| 입력 | 동작 |
|------|------|
| WASD / 방향키 | 플레이어 이동 |
| 마우스 좌클릭 홀드 | 연속 발사 |

### 게임 규칙
- 플레이어는 화면 경계 내부에서만 이동합니다.
- 적 또는 적 총알에 충돌하면 라이프가 감소합니다.
- 라이프는 총 3개이며, 모두 소진 시 게임오버가 발생합니다.
- 적 처치 시 점수가 증가하고 UI에 즉시 반영됩니다.
- Retry 버튼으로 현재 씬을 다시 로드해 재시작할 수 있습니다.

---

## 📁 프로젝트 구조

```text
spaceShooters/
├── Assets/
│   ├── Player.cs                 # 플레이어 이동/사격/피격
│   ├── EnemyController.cs        # 적 이동/피격/처치 점수
│   ├── EnemyBullit.cs            # 적 조준 및 발사 루틴
│   ├── EnemyBullitController.cs  # 적 총알 이동/충돌 처리
│   ├── BullitController.cs       # 플레이어 총알 이동/삭제
│   ├── SpawnPoint.cs             # 다중 스폰 포인트 기반 적 생성
│   ├── EnemyGenerate.cs          # 랜덤 타이머 기반 적 생성
│   ├── GameOver.cs               # 라이프/점수/게임오버/재시작 UI
│   ├── DrawArrow.cs              # 스폰 방향 Gizmo 디버그
│   ├── Prefabs/                  # 플레이어/적/총알/파워 프리팹
│   └── Scenes/GameScene.unity    # 메인 플레이 씬
├── Packages/manifest.json        # 패키지 의존성
└── ProjectSettings/              # Unity 프로젝트 설정
```

---

## 📝 스크립트 설명

### Player.cs
- 입력 축(Horizontal, Vertical) 기반 이동
- 이동 방향에 따라 Animator State(Idle/Left/Right) 전환
- fireRate 간격으로 마우스 홀드 연사
- 피격 시 비활성화 후 GameOver 로직으로 라이프 처리

### EnemyController.cs
- moveDirection 기반 적 이동 (상단/측면/대각)
- 피격 시 짧은 히트 스프라이트 표시
- HP 0 이하 시 점수 추가 후 적 오브젝트 제거

### EnemyBullit.cs / EnemyBullitController.cs
- 플레이어 위치를 조준한 방향 벡터 계산
- 2연사 후 대기, 재조준 반복 패턴 발사
- 탄환은 화면 밖으로 벗어나면 자동 제거

### SpawnPoint.cs
- 여러 스폰 포인트에서 주기적으로 적 생성
- 일부 포인트는 목표 지점 방향 벡터를 사용해 대각선 진입
- Gizmo와 디버그 화살표로 진입 방향 시각화

### GameOver.cs
- 라이프 UI(3칸) 관리
- 점수 누적 및 게임오버 패널 표시
- 게임오버 시 적/총알 정리 후 Retry로 씬 재시작

---

## ⚔️ 적 타입 & 점수 시스템

| 요소 | 설명 |
|------|------|
| 적 체력 | EnemyController의 hp 값으로 관리 |
| 이동 패턴 | SpawnPoint 설정에 따라 하강/좌우/대각 진입 |
| 처치 점수 | EnemyController의 score 값이 누적 |
| 점수 표시 | GameOver의 scoreText(TextMeshProUGUI)로 출력 |

---

## 🔫 플레이어 공격 시스템

| 요소 | 설명 |
|------|------|
| 발사 조건 | 마우스 좌클릭 홀드 |
| 발사 주기 | Player.fireRate (기본 0.2초) |
| 기본 탄환 | BullitController가 위쪽 직선 이동 |
| 탄환 정리 | 화면 상단 이탈 시 제거 |

추가 프리팹(Power1/2/3)은 프로젝트 내에 포함되어 있으며, 확장형 공격 패턴 실험에 활용할 수 있습니다.

---

## 🧩 UI 시스템

- 현재 점수: 실시간 업데이트
- 라이프 아이콘 3개: 피격 시 순차 비활성화
- 게임오버 패널: 최종 점수 표시
- Retry 버튼: 현재 씬 즉시 재시작

---

## 💻 설치 및 실행

### 요구 사항
- Unity 6000.4.1f1 이상

### 실행 방법
1. 저장소 클론

```bash
git clone https://github.com/Masil21/2d-vertical-shooter.git
```

2. Unity Hub에서 클론 폴더 열기
3. Assets/Scenes/GameScene.unity 씬 열기
4. Play 버튼으로 실행

---

프로젝트 기반 README 형식은 sinsnghwan 저장소 스타일을 참고해, 현재 코드 구조와 실제 동작에 맞게 재구성했습니다.
