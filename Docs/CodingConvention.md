
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

