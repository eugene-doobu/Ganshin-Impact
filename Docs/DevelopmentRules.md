
## 1. Debug 규칙

GanDebugger.Log를 사용하며, 로그 사용시 해당 클래스의 최상위 네임스페이스를 제외한 네임스페이스를 표기

혹은 특정 피쳐에 종속된 헬퍼 메서드를 제작하여 사용한다.

### 예시


```C#
// in namespace GanShin.Creature
GanDebuger.LogError($"[{nameof(Creature)}] actionMap is null!");
```

## 2. Zenject 관련

- 매니저격 클래스는 ProjectInstaller에서 바인딩
- 매니저격 클래스는 AsSingle().NonLazy()로 구현
- 가능하면 Unity Event를 사용하지 않고 구현
- 유니티 이벤트가 꼭 필요한 경우가 아닌 이상 MonoBehaviour 사용 최소화

### Unity Event 대응

- Awake : 생성자
- Start : IInitializable
- Update : ITickable
- ILateTickable : ILateTickable
- OnDestroty : IDisposal

TriggerEnter와 같은 이벤트는 MonoBehaviour 상속을 받아서 구현

## 3. 비동기처리

(여기서 비동기의 뜻은, 코드상에서 요청 후 결과를 기다리지 않고 다음 라인의 작업을 진행하는 것을 의미. 따라서 비동기 처리에 코루틴도 포함됨)

UniTask 활용

코루틴을 사용하지 않음, 유니티 내부 구현상 코루틴을 사용하는 경우에도 uniTask로 변환하여 사용

## 4. UI MVVM

UI는 DataBinder를 이용하여 MVVM 으로 구현

1. view와 viewModel은 DataBinder를 통해 서로를 구독하여 데이터를 전송
1. viewModel은 dataBinderContext로 표현
