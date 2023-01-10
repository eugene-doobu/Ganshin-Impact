
## 1. Debug 규칙

GanDebugger.Log를 사용하며, 로그 사용시 해당 클래스의 최상위 네임스페이스를 제외한 네임스페이스를 표기

혹은 특정 피쳐에 종속된 헬퍼 메서드를 제작하여 사용한다.

### 예시


```C#
// in namespace GanShin.Creature
GanDebuger.LogError($"[{nameof(Creature)}] actionMap is null!");
```
