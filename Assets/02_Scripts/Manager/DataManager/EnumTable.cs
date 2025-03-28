public enum OBJ_TYPE 
{ 
	BUILDING                     = 0,        	// 빌딩
	CHARACTER                    = 1,        	// 캐릭터
}
public enum ITEM_STATUS 
{ 
	NONE                         = 0,        	// 없음
	BOXED                        = 1,        	// 박스 밑에 거미줄
	SPIDERWEB                    = 2,        	// 거미줄만 있음
}
public enum SLOT_ANIM_TYPE 
{ 
	ACTIVE                       = 1,        	// 활성화 됨
	UNACTIVE                     = 2,        	// 비활성화 됨
}
public enum UNIT_TYPE 
{ 
	TANKER                       = 0,        	// 탱커
	ARCHER                       = 1,        	// 궁수
}
public enum ITEM_TYPE 
{ 
	UNIT                         = 0,        	// 조각
	SOUL                         = 1,        	// 영혼 아이템 (재화)
	EXP                          = 2,        	// 경험치
	STAMINA                      = 3,        	// 스태미나
	GOLD                         = 4,        	// 골드
	ADFREE                       = 5,        	// adfree
}
public enum RARITY_TYPE 
{ 
	COMMON                       = 0,        	// 
	RARE                         = 1,        	// 
	EPIC                         = 2,        	// 
	LEGENDARY                    = 3,        	// 
}
public enum LANGUAGE_TYPE 
{ 
	KO                           = 0,        	// ios_3166-1 언어코드
	EN                           = 1,        	// 
	JP                           = 2,        	// 
	CN                           = 3,        	// 
	TW                           = 4,        	// 
	PT                           = 5,        	// 
	FR                           = 6,        	// 
	DE                           = 7,        	// 
	RU                           = 8,        	// 
}
public enum TUTO_TYPE 
{ 
	CUTSCENE                     = 0,        	// 
	DIALOUGE                     = 1,        	// 
	CAMERASTAGEMOVE              = 2,        	// 
	NEEDTOUCH                    = 3,        	// 
	ATTENDANCE                   = 4,        	// 
}
public enum EVENT_TYPE 
{ 
	SPAWN                        = 0,        	// 스폰 이벤트
	NEXTSTAGE                    = 1,        	// 다음스테이지 이벤트
	STORY                        = 2,        	// 이야기진행 이벤트
	DAMAGE                       = 3,        	// 데미지 이벤트
}
public enum SPAWN_TYPE 
{ 
	STAGE_ENTRY_SPAWN            = 0,        	// 스테이지 진입 스폰
	KILL_MONSTER_SPAWN           = 1,        	// 몬스터 처치 스폰
}
public enum SPAWN_TARGET 
{ 
	USER                         = 0,        	// 유저
	ENEMY                        = 1,        	// 적
	ENTITY                       = 2,        	// 대상(파괴가능 오브젝트등)
	NPC                          = 3,        	// 상호작용 NPC
	REWARD                       = 4,        	// 보상(획득가능한 재화나 아이템등)
}
