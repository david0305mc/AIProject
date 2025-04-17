public enum OBJ_TYPE
{
	BUILDING                     = 0         , // 빌딩
	CHARACTER                    = 1         , // 캐릭터
}
public enum ITEM_STATUS
{
	NONE                         = 0         , // 없음
	BOXED                        = 1         , // 박스 밑에 거미줄
	SPIDERWEB                    = 2         , // 거미줄만 있음
}
public enum SLOT_ANIM_TYPE
{
	ACTIVE                       = 1         , // 활성화 됨
	UNACTIVE                     = 2         , // 비활성화 됨
}
public enum UNIT_TYPE
{
	TANKER                       = 0         , // 탱커
	ARCHER                       = 1         , // 궁수
}
public enum ITEM_TYPE
{
	GOLD                         = 1         , // 골드
	GEM                          = 3         , // 보석
	DUNGEONITEM                  = 4         , // 던전 아이템
	REPUTATION                   = 5         , // 평판수치
	EQUIPITEM                    = 10        , // 장비
	CHARACTER                    = 20        , // 캐릭터
	DUNGEONHEART                 = 101       , // 던전하트
}
public enum RARITY_TYPE
{
	COMMON                       = 0         , // 
	RARE                         = 1         , // 
	EPIC                         = 2         , // 
	LEGENDARY                    = 3         , // 
}
public enum LANGUAGE_TYPE
{
	KO                           = 0         , // ios_3166-1 언어코드
	EN                           = 1         , // 
	JP                           = 2         , // 
	CN                           = 3         , // 
	TW                           = 4         , // 
	PT                           = 5         , // 
	FR                           = 6         , // 
	DE                           = 7         , // 
	RU                           = 8         , // 
}
public enum TUTO_TYPE
{
	CUTSCENE                     = 0         , // 
	DIALOUGE                     = 1         , // 
	CAMERASTAGEMOVE              = 2         , // 
	NEEDTOUCH                    = 3         , // 
	ATTENDANCE                   = 4         , // 
}
public enum MAP_EVENT_TYPE
{
	SPAWN                        = 0         , // 스폰 이벤트
	NEXTSTAGE                    = 1         , // 다음스테이지 이벤트
	STORY                        = 2         , // 이야기진행 이벤트
	BOX                          = 3         , // 
}
public enum ACTION_TYPE
{
	MOVE                         = 1001      , // 
	ATTACK                       = 2001      , // 
	TACKLE                       = 2002      , // 
	SUMMON                       = 3001      , // 
}
public enum MOVE_TYPE
{
	STOP                         = 1         , // 
	RANDOMMOVE                   = 2         , // 
	EXITMOVE                     = 3         , // 
}
public enum ATTRIBUTE_TYPE
{
	ATK                          = 1         , // 
	ATKSPEED                     = 2         , // 
	HP                           = 3         , // 
	MOVESPEED                    = 4         , // 
	DEF                          = 5         , // 
	CRITICALCHANCE               = 6         , // 
	CRITICALATK                  = 7         , // 
	PROJECTILESCALE              = 8         , // 
	PROJECTILESPEED              = 9         , // 
	DODGEMAX                     = 10        , // 
	DODGECHARGETIME              = 11        , // 
	HPRECOVERY                   = 12        , // 
	EVASION                      = 13        , // 
	INVINCIBLE                   = 14        , // 
	HITFREEZE                    = 50        , // 
	HITMOVESPEED                 = 51        , // 
	MULTISHOT                    = 100       , // 
	BOUNCESHOT                   = 101       , // 
	DOTDMG                       = 102       , // 
	PIERCE                       = 103       , // 
	TRACKING                     = 104       , // 
	DOUBLESHOT                   = 105       , // 
	DIAGONALSHOT                 = 106       , // 
	EXTRAFIRE                    = 107       , // 
	WALLBOUNCESHOT               = 108       , // 
	RANDOMATTACK                 = 109       , // 
	SUMMONOBJ                    = 200       , // 
	SECONDPROJECTILE             = 201       , // 
	DASHPROJECTILE               = 202       , // 
	FIREPROJECTILE               = 203       , // 
}
public enum EQUIPMENT_TYPE
{
	WEAPON                       = 1         , // 
	HELMET                       = 2         , // 
	ARMOR                        = 3         , // 
	NECKLACE                     = 4         , // 
	RING                         = 5         , // 
	BOOTS                        = 6         , // 
}
public enum DUNGEON_EVENT_TYPE
{
	DIALOG                       = 1         , // 
	BATTLE                       = 2         , // 
	BUFF                         = 3         , // 
}
public enum ABILITY_CONDITION_TYPE
{
	NONE                         = 0         , // 
	HPCHECK                      = 1         , // 
	TIMER                        = 2         , // 
	KILLENEMY                    = 3         , // 
}
public enum OBJECT_TYPE
{
	NORMAL                       = 1         , // 
	TRAP                         = 2         , // 
}
