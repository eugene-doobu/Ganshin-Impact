# Addressable Label

Addressable Group에서 에셋들을 로드하는 기준이 되는 관리 단위.

Addressable 리소스는 크게 두 유형으로 관리함(Resourcemanager코드 참고)

- dontDestroyOnLoadResources: 한번 로드하면 프로세스 종료시까지 유지되는 리소스
- resources: 로드 후 씬 전환시마다 제거되는 리소스

Addressable은 일반적으로 Label단위로 로드되며, Label에 따라 프로세스 초기화시 로드되어 dontDestroyOnLoadResources 컬렉션에서 관리되거나, 씬 로드 혹은 리소스 요청시에 로드되어 씬 전환시 제거되는 resources 컬렉션에 관리되는 것으로 구분된다.

## Label

### default label

Addressable 패키지 설치시 기본적으로 있는 레이블. 사용하지 않는다

 ### Data

ScriptableObject형태의 설정 파일들을 포함하는 레이블
 
- 로드시점: 프로젝트 초기화
- 관리 컬렉션: dontDestroyOnLoadResources

### GlobalUI

GlobalUI 에셋들에 적용되는 Label. 어느 씬에서도 접근이 가능해야 하기 때문에 프로젝트 초기화시 한번 로드하고 해제하지 않는다. LoadingScene관련 에셋들도 이 Label에 포함된다.
 
- 로드시점: 프로젝트 초기화
- 관리 컬렉션: dontDestroyOnLoadResources

### Intro

IntroScene에서 사용되는 에셋들을 관리하는 레이블
 
- 로드시점: IntroScene 초기화시
- 관리 컬렉션: resources

### Space

SpaceScene에서 사용되는 에셋들. 게임 대부분의 시간을 Space씬에서 보내기도 하고, 해당 에셋들을 반복하여 해제하고 로드하는게 불필요하다 생각하여 프로젝트 초기화 시점에서 한번에 로드한다.
 
- 로드시점: 프로젝트 초기화
- 관리 컬렉션: dontDestroyOnLoadResources

### Vilage

VilageScene에서 사용되는 에셋들을 관리하는 레이블, Space Scene과 함께 초기화된다.
 
- 로드시점: VilageScene 초기화시
- 관리 컬렉션: resources

### 던전

던전 씬마다 별도의 Label을 보유하며, Vilage씬과 동일한 형태로 초기화된다.