<!--***Auto-Open Markdown Preview*** 이거 깔아서 보셈 ㅇㅇ-->

# **몬스터 감정 시스템 아키텍처 설계 문서**

## **개요 (Overview)**

본 시스템은 **유한 상태 기계(FSM)**와 **디자인 패턴**을 활용하여 몬스터의 감정 상태를 관리하고, 플레이어의 상호작용에 따라 유연하게 상태를 전환하기 위해 설계되었습니다.

본 아키텍처는 **느슨한 결합(Loose Coupling)**과 **확장성**을 확보하는 것을 목표로 합니다.

## 

## **주요 구성 요소 및 역할**

| 파일명 | 주요 역할 | 디자인 패턴 | 설명 |
| ----- | ----- | ----- | ----- |
| **`Emotion.cs`** | **정의 (Definition)** | Enum | 몬스터가 가질 수 있는 모든 기본 및 합성 감정 타입(`EmotionType`)을 정의합니다. `Neutral` 상태를 포함하여 초기 상태 및 합성 실패를 처리합니다. |
| **`IEmotionState.cs`** | **규격 (Interface)** | State Pattern | 모든 감정 상태 클래스가 따라야 할 기본 규격(`OnEnter`, `UpdateState`, `OnExit`)을 정의합니다. |
| **`JoyState.cs`** (및 기타 State 클래스) | **행동 (Behavior)** | State Pattern | `IEmotionState`를 구현하여, 해당 감정(`Joy`, `Rage` 등)에서 몬스터가 **어떻게 행동할지**에 대한 로직을 오직 이 클래스 내부에서만 처리합니다. |
| **`EmotionFactory.cs`** | **생성/관리 (Factory)** | Factory Pattern | `EmotionType`을 입력받아 해당 **상태 객체(`IEmotionState`)의 인스턴스를 제공**합니다. `Dictionary` 캐싱을 통해 객체 생성 부하를 줄이고 OCP 원칙을 준수하도록합니다. |
| **`EmotionTable.cs`** | **규칙 (Rule)** | Data-Driven | 두 감정(`Current` \+ `Added`)이 조합될 때 어떤 **합성 감정**이 나와야 하는지를 정의하는 데이터 테이블(Dictionary) 역할을 수행합니다. |
| **`PlayerEmotionController.cs`** | **이벤트 발행자 (Publisher)** | Event Pattern | 플레이어의 입력(클릭/키 입력)을 감지하여 **감정 주입 요청 이벤트**를 발행(Invoke)합니다. 다른 모듈과 직접 연결되지 않아 결합도가 낮습니다. |
| **`Monster.cs`** | **중앙 FSM / 이벤트 구독자** | State & Event Pattern | **현재 감정 상태(`_currentState`)를 관리**하며, 외부에서 들어온 감정 이벤트(MonsterEmotionManager.OnEmotionAppliedToMonster)를 **구독**하여 자신에게 온 요청만 처리합니다. |

## **시스템 흐름 및 관계 (Decoupled Flow)**

이 시스템은 **이벤트 패턴**을 통해 각 모듈의 역할을 분리하고 있습니다.

1. **요청 발생 (Player):**  
   * `PlayerEmotionController`가 몬스터를 레이캐스트로 감지한 후, **`MonsterEmotionManager.OnEmotionAppliedToMonster`** 이벤트를 호출(발행)합니다.  
   * **중요:** `PlayerEmotionController`는 몬스터를 찾았다는 사실만 알릴 뿐, 누가 이 요청을 처리하는지는 알지 못합니다. (느슨한 결합)  
2. **이벤트 구독 (Monster):**  
   * 씬에 존재하는 **모든 `Monster` 객체**는 `Start()` 시점에 이 정적 이벤트를 구독하고 있습니다.  
   * 이벤트가 발동되면, 각 몬스터는 페이로드(Payload)를 확인하여 **"이 요청이 나에게 온 것인지"**를 판단하고, 자신에게 온 요청이라면 `MonsterEmotionManager.HandleEmotionApplied`를 호출합니다.  
3. **로직 처리 (EmotionManager & EmotionTable):**  
   * `MonsterEmotionManager`는 몬스터의 현재 감정과 플레이어가 주입한 감정을 `EmotionTable.Mix()` 함수에 전달합니다.  
   * `EmotionTable`은 조합 규칙을 계산하여 최종 감정 타입(`finalEmotion`)을 반환합니다.  
4. **상태 전환 (Monster & Factory):**  
   * `Monster`는 합성된 `finalEmotion`을 전달받아 `SetEmotion()`을 실행합니다.  
   * `SetEmotion()` 내부에서 **`EmotionFactory.Create()`**를 호출하여, **미리 캐싱된** 새로운 `IEmotionState` 객체를 가져와 `_currentState`에 할당합니다.  
   * **참고:** `EmotionFactory`는 정적 생성자(Static Constructor)를 사용하여 **클래스 최초 사용 시** 모든 상태 객체를 한 번만 초기화합니다.

## 

## **협업 가이드: 개발자를 위한 확장 포인트**

이 아키텍처는 새로운 감정이나 행동을 추가할 때 기존 코드를 수정할 필요가 없도록 설계되었습니다 (OCP 원칙).

### **1\. 새로운 감정 상태 추가 시 (행동 담당자)**

1. **`Emotion.cs`:** 새로운 감정(`EmotionType.Surprise` 등)을 `enum`에 추가합니다.  
2. **새 스크립트 생성:** `SurpriseState.cs` 파일을 생성하고, **`IEmotionState`** 인터페이스를 구현하여 `OnEnter`, `UpdateState`, `OnExit` 로직을 정의합니다.

### **2\. 상태 관리 설정 추가 시 (아키텍트 담당자)**

1. **`EmotionTable.cs`:** `_mixTable`에 새로운 감정 조합 규칙을 추가합니다.  
   * 예: `{(EmotionType.Joy, EmotionType.Fear), EmotionType.Surprise}`  
2. **`EmotionFactory.cs`:** `EmotionFactory`의 정적 생성자에 새로 만든 상태 객체를 캐싱하도록 등록합니다.  
   * `_stateCache.Add(EmotionType.Surprise, new SurpriseState());`

### **3\. Input 방식 변경 시 (Input 담당자)**

1. **`PlayerEmotionController.cs`:** 이 파일만 수정하여 `Update()` 루프 내에서 새로운 입력 방식을 추가하고, 최종적으로 **`MonsterEmotionManager.OnEmotionAppliedToMonster.Invoke(...)`**를 호출하기**만** 하면 됩니다. 다른 시스템에 영향을 주지 않습니다.