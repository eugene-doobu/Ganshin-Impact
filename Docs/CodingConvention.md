
## 1. Debug 규칙

Debug.Log를 사용하며, 로그 사용시 해당 클래스의 최상위 네임스페이스를 제외한 네임스페이스를 표기


### 예시


```C#
// in namespace GanShin.Creature
Debug.LogError($"[{nameof(Creature)}] actionMap is null!");
```

