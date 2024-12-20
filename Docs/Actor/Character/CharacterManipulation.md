# 캐릭터 조작

유저가 조작(키보드, 마우스)시에 일어나는 동작을 정리한 문서

## 캐릭터

3가지 캐릭터가 있으며, 상황에 따라 바꿔가며 사용할 수 있는 형태

캐릭터마다 가능한 전용 동작이 존재하며, 해당 기능을 통해서만 진행가능한 루트가 있음

-> 현재 캐릭터 당 전용 동작은 맵 컨텐츠와 같이 다른 문서(캐릭터 전용 동작)에서 작성 예정

## 캐릭터 변경

키보드 상단 숫자 키 1, 2, 3 을 입력하여 캐릭터를 변경할 수 있다.

1. [AI](./Ai.md)
1. [Riko](./Riko.md)
1. [MuscleCat](MuscleCat.md)

## 이동방식

카메라가 바라보는 방향을 XZ평면에 투영한후 정규화한 벡터를 '전방' 으로 정의

### 이동키 : WASD

이동 키를 입력 시 이동 키에 해당하는 방향으로 캐릭터가 회전하며 해당 방향으로 전진함

W - 전방, A - 왼쪽, S - 후방, D - 오른쪽

### 특수행동 : Shift

Shift를 누르면 캐릭터마다 가지고 있는 특수행동을 수행한다.

### 구르기 : Space

구르기를 사용한다. 특정 패턴을 피하거나 장애물을 넘는데 사용한다.

구르기는 스태미너를 소모한다 (소모량 20)

## 스태미너

대쉬를 할 수 있는 게이지

게이지는 게이지가 소모되는 동작을 일정 시간 이상 하지 않으면 채워진다.

스태미너 최대치 : 100

게이지 충전 시간 : {n}초

게이지 소모 후 충전시간 딜레이 : {f}초

## 공격 및 스킬

공격 및 스킬에 대한 자세한 내용은 각 캐릭터 문서 참고

### 기본공격 : 마우스 좌클릭

캐릭터 마다 고유의 기본 공격을 이용한다.

### 기본스킬 : E키

캐릭터 별로 가지고 있는 기본 스킬을 사용.

각 캐릭터별 특징을 가장 잘 나타내는것이 스킬

### 궁극스킬 : Q키

캐릭터 별로 가지고 있는 궁극 스킬을 사용

궁극 스킬은 별도의 쿨타임은 없으며, 궁극 게이지를 채워야 사용할 수 있다.

## 특수행동

### 상호작용 : F키

NPC등 상호작용할 수 있는 대상과 상호작용 할 수 있는 단축키
