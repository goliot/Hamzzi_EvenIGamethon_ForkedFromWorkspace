# EvenIGamethon

## 주의사항
1. Push 이전에 항상 **pull** 할것 있는지 확인하기
2. **Scene 작업** 전에는 항상 Scene 관리자에게 물어보고 작업하기
3. 작업 내용의 **주석**은 최대한 상세히
4. **상속 구조로 이루어진 스크립트**는 기존의 변수명 수정은 최대한 삼가고 불가피하게 작업을 해야하는 경우는 팀원들에게 미리 알리고 주석을 상세히 달기
5. 개발 버전 업데이트 규칙
   - 최신 업데이트를 **최상단**에 배치

|V.1.0.0  | 1             | 0            |  0           | JS      | 20240108 |
|:-------:|:-------------:|:------------:|:------------:|:-------:|:-------:|
|   의미  | 개발일정 페이즈 | 씬수정       | 스크립트 작업 |작업자    |  날짜  |

6. 개발 일정 페이즈  

|구간   |    Phase1    |     Phase2   |      Phase3   |     Phase4    |
|:----:|:------------:|:------------:|:-------------:|:-------------:|
|기간| 01.08 ~ 01.26 | 01.27 ~ 02.02 | 02.03 ~ 02.09 | 02.10 ~ 02.23 |
|개요| 게임 구현 | 서버 구현(구글플레이 제외) | 서버 구현(구글플레이 포함) | 최종 QA, 버그 수정, 추가적인 시스템 구현 및 개발 최적화| 
---
---
## V.1.0.4 - SM 2024-01-12
- 자동공격 로직 작성
- 스킬에 따른 다른 쿨타임 독자적으로 돌아가도록 작성
- 몬스터 Xml 데이터 파싱 적용
- 벽 체력 설정
	- 벽 피격 로직
---
## V.1.0.3 - SM 2024-01-11
#### Battle_Proto 씬 작업
- 총알 오브젝트 풀링
    - 몬스터의 오브젝트 풀링과 같은 방식으로 작동
- 플레이어 총알 발사 로직
    - ##### 오류 수정 사항
    - 발사 시 발사 방향으로 회전된 스프라이트가 나오도록 처리
    - 총알이 날아가는 도중에 적이 죽어버리는 경우 처리
    - 가장 가까운 적을 찾는 함수가 비활성화된 객체들까지 포함해서 계산하는 오류 처리
- 몬스터 피격, 사망 로직
    - 몬스터 피격시 데미지를 받고, 사망하도록(SetActice(false))
- XML 파싱 틀 제작
    - 현재는 메인 캐릭터의 데이터가 파싱되도록 처리됨
    - 추후 몬스터쪽에도 적용할 예정
<img src = "https://github.com/Jinlee0206/EvenIGamethon/assets/109404269/7fae7317-bc98-45bc-95ce-b5580492e40a" width = "180" height = "320">
  
---
## V.1.0.2 - JS 2024-01-11
- 싱글톤 제네릭 업데이트
  - 재생산성을 높이기 위해 제네릭 타입으로 싱글톤 스크립트 작성완료, 게임 매니저에 적용
- UI 작업
  - UIManager 스크립트 작성
    - GameManager 호출 이후 UIManager를 생성하고, GameManager와 커플링 없이 UI 업무는 UIManager에서 독립적으로 실행할 수 있게 구현 예정
  - CardUI
    - 특정 조건(레벨업)이 되었을 때, CardUI가 오픈되고 게임은 일시 정지된다.
    - 세가지 선택지 중에 하나를 선택하였을 때, 해당 카드의 효과를 스테이지 상에 즉각 업데이트하며(미구현) 게임 플레이가 재개된다.
    - Card 선택 메소드는 따로 로직 작성할 수 있게 스크립트 분할 작업
  - GameSpeedUI
    - 기본 배속, 1.5배속 버튼 형식으로 추가, 후에 Asset 정해지면 이미지 토글 형식으로 대체 예정
  - PauseUI
    - Home
    - Resume
    - Retry
---
## V.1.0.1 - SM 2024-01-10
#### Battle_Proto 씬 작업
- 적 오브젝트 풀링 틀 작성
    - 적 객체가 사망 시 Destroy()가 아닌 SetActive(false) 시켜서 씬에 남아있게 함
    - 다음 적이 생성될 때 현재 비활성화된 객체가 있는 경우 해당 객체를 재활용해서 생성
    - 없는 경우에는 새로 생성
    - 적은 프리팹 하나로 구현
        - RuntimeAnimatorController만 바꾸면 외형도 바뀌게 작업
        - 몬스터 능력치는 SpawnData로 삽입됨 -> 추후 Xml로 삽입하도록 변경 예정
    - PoolManager에는 위에서 적은 프리팹 정보가 들어있음
    - Spawner에는 SpawnData 배열로 능력치가 저장되어있음
    - 해당 정보들을 불러와서 몬스터를 생성하는 방식
- 적 움직임 작성
    - 적 움직임은 Y축으로 내려오는 움직임만 필요하므로 해당 부분 작성
- 각 웨이브 시간마다 다음 웨이브가 몰려오도록 작성
<img src = "https://github.com/Jinlee0206/Jinlee0206.github.io/assets/105345909/ae6f4c1b-b2de-4e65-a4fb-89fe67223a1a" width = "180" height = "320">
  
---
## V.1.0.0 - JS 2024-01-10
- 전투 기획 초안
  <details>
  <summary> 접기/펼치기 </summary>  
  <img src = "https://github.com/Jinlee0206/EvenIGamethon/assets/109404269/f2f72556-ab26-4a0d-860c-51dd179601a8" width = "420" height = "930">
 </details>

- 해상도 조절  
  - 9 * 16 모바일 비율 임시 설정 (택)
  - 9 * 19 플립 같은 종횡비가 큰 비율

- UI 작업
  - Title UI
    - 임시 타이틀로 간단하게 배경과 이미지, 타이틀, 그리고 시작 문구로 간단 제장  
   <img src = "https://github.com/Jinlee0206/Jinlee0206.github.io/assets/105345909/a1f63735-e03f-4134-b89e-c71f951d7c5e" width = "180" height = "320">

  - Stage UI
    - ScoreUI
      - 획득 점수 / 플레이 속도 조절 버튼 (임시)
        - 점수는 Int 형으로 Text 받는 식으로 GameManager 구축 후 추후 연동
        - 플레이 속도 조절은 버튼으로 구현할 예정 => 버튼을 누를 때마다 이미지 변경되게 구현 예정 
      - Wave  
        - 현재 웨이브와 해당 스테이지 총 웨이브 표기
      - Timer
        - 해당 스테이지 시작부터 타이머 쭉 흘러가게 설정
      - Exp Bar
        - 슬라이더로 제작
        - 왼쪽에서 오른쪽으로 채워지는 형식으로 구현
        - 100% 채워질 시 카드 오픈 UI 뜨게 구현 예정
        - 현재 레벨을 알 수 있게 레벨 표시도 진행하게 기획 수정 요청 필요
    - DefenseUI
      - SpawnPoint
        - 중앙에 위치한 스폰 포인트는 기본 햄찌가 소환될 곳 (default)
        - 양 옆에 서브 햄찌가 소환될 공간을 미리 만들어두고 클릭해서 건설할 수 있는 타워 디펜스 형식의 구현 예정  
       
    - AttackUI
      - 해금되는 기본 공격 스킬 탭
      - 쿨타임이 돌면 자동으로 공격이 실행되고 쿨타임 아이콘 UI 보여질 예정
     <p align="center"> <img src = "https://github.com/Jinlee0206/Jinlee0206.github.io/assets/105345909/87342756-c30a-4d58-b753-39cf4a6e4f3e" width = "40" height = "40">

    - 최종 결과   
    <p align="center"> <img src = "https://github.com/Jinlee0206/Jinlee0206.github.io/assets/105345909/6caae26b-e6ab-4e0d-a905-7df65e6b70b9" width = "180" height = "320">
    
---

### V.0.0.0 - JS 2024-01-08
- 개발 초기 셋업
  - Unity Version matching : V22.3.4.f1  
  - Asset Serialization Mode : Force Text  
  - Git Repository set up : 개인 레포지토리에 개설, public으로 사용 예정  
  - Git LFS installed and checked -> success  
  - Test Project 생성  
    - 개인 Layout 생성

- Git, Github 비개발자 직군 간단 세미나

- Asset Searching
  - 상위 기획 자료 미팅 (2024.01.08 22:00 예정) 이후 방향성 확인 이후 어울릴 만한 Asset search
   
- 공동 작업용 변수명 통일 기준 확립
  - [C# 변수 명명법](https://jinlee0206.github.io/develop/Naming.html)
  - 폴더 분류 기준 확립

- 개발자 간 개발 작업 분업화 미팅
  - 대주제 : 프로토 타입단계에서 Scene 담당, 수학 로직, 이펙트, UI, ... , etc.
