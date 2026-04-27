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

## At a Glance

| 구분 | 내용 |
| --- | --- |
| 한 줄 설명 | 위에서 내려오는 적을 격파하며 생존 점수를 쌓는 종스크롤 슈팅 게임 |
| 핵심 재미 | 회피, 연사, 파워업, 아이템 수집, Boom을 활용한 긴급 화면 정리 |
| 현재 구현 | 플레이어 이동, 연속 발사, 적 생성, 아이템 드롭, 파워 강화, Boom 시스템, 게임오버 UI, 배경 스크롤 |
| 플레이 목표 | 최대한 오래 살아남아 높은 점수를 기록하는 것 |

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

## Gameplay Flow

`Move` -> `Shoot` -> `Defeat Enemies` -> `Collect Items` -> `Power Up or Store Boom` -> `Use Boom in Danger` -> `Survive Longer` -> `Reach Higher Score`

## Controls

| 입력 | 동작 |
| --- | --- |
| WASD / 방향키 | 플레이어 이동 |
| 마우스 좌클릭 홀드 | 연속 발사 |
| 마우스 우클릭 | Boom 사용 |

## Core Systems

### 1. Shooting and Power Level

- 기본 공격은 정면 연사입니다.
- Power 아이템을 획득하면 무기 단계가 상승합니다.
- 최대 3단계까지 강화되며, 단계에 따라 발사 프리팹이 달라집니다.

### 2. Boom System

- Boom 아이템을 획득하면 최대 3개까지 저장됩니다.
- 우클릭으로 즉시 발동할 수 있습니다.
- 발동 시 폭발 이펙트가 재생됩니다.
- 일정 시간 동안 적에게 큰 피해를 반복 적용합니다.
- 같은 시간 동안 화면의 적 탄환을 제거합니다.
- 사용 후 Boom UI가 즉시 갱신됩니다.

### 3. Item Drop System

적 처치 시 아이템이 확률적으로 생성됩니다.

| 드롭 결과 | 확률 | 효과 |
| --- | --- | --- |
| 없음 | 30% | 추가 보상 없음 |
| Coin | 30% | 점수 +100 |
| Power | 20% | 점수 +500, 화력 강화 |
| Boom | 20% | 점수 +200, Boom +1 |

### 4. UI Feedback

- Score UI: 현재 점수를 실시간으로 표시
- Life UI: 남은 라이프 3칸을 시각적으로 관리
- Boom UI: Boom 보유 개수를 별도 아이콘으로 표시
- Game Over UI: 최종 점수와 Retry 버튼 제공

### 5. Background Scrolling

- 3장의 배경 SpriteRenderer를 순환 배치해 자동 스크롤을 구현했습니다.
- 씬이 정지된 화면이 아니라 계속 전진하는 슈팅 게임처럼 느껴지도록 구성했습니다.

## Latest Updates

### 2026-04-27 기준 반영 내용

- Boom 애니메이션과 Boom 전용 프리팹 추가
- Player, GameOver, ItemManager3 로직 확장
- Boom 획득, 보유, 사용, UI 반영 흐름 연결
- 아이템 드롭을 확률형 구조로 변경
- GameScene 프리팹/씬 구성 업데이트
- BackgroundScroller 추가로 자동 배경 스크롤 구현

## Project Overview

| 항목 | 내용 |
| --- | --- |
| 프로젝트명 | 2d-vertical-shooter |
| 장르 | 2D Vertical Shooter |
| 엔진 | Unity 6000.4.1f1 |
| 렌더링 | Universal Render Pipeline |
| 언어 | C# |

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
│   │   ├── BackgroundScroller.cs
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

1. 저장소를 클론합니다.

```bash
git clone https://github.com/Masil21/2d-vertical-shooter.git
```

2. Unity Hub에서 프로젝트 폴더를 엽니다.
3. `Assets/Scenes/GameScene.unity` 씬을 엽니다.
4. Play로 실행합니다.

## Recent Commit Trail

- `a6726d8` Docs: refresh README with latest gameplay updates
- `848526f` Update changes
- `903b5f3` Update scenes, scripts, animations, and prefabs
- `a750ee6` Restructure project assets and scripts
