#pragma warning disable 114
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager {
	public partial class Localization {
		public string id;
		public string ko;
		public string en;
		public string jp;
	}
	public Localization[] LocalizationArray { get; private set; }
	public Dictionary<string, Localization> LocalizationDic { get; private set; }
	public void BindLocalizationData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(LocalizationArray)).SetValue(this, deserializedData, null);
		LocalizationDic = LocalizationArray.ToDictionary(i => i.id);
	}
	public Localization GetLocalizationData(string _id) {
		if (LocalizationDic.TryGetValue(_id, out Localization value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class Level {
		public int id;
		public int level;
		public int exp;
		public int unlockslot;
		public int goldreward;
	}
	public Level[] LevelArray { get; private set; }
	public Dictionary<int, Level> LevelDic { get; private set; }
	public void BindLevelData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(LevelArray)).SetValue(this, deserializedData, null);
		LevelDic = LevelArray.ToDictionary(i => i.id);
	}
	public Level GetLevelData(int _id) {
		if (LevelDic.TryGetValue(_id, out Level value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class StageInfo {
		public int themeid;
		public int id;
		public string prefabname;
		public string msconditiontype;
		public string msconditionvalue;
		public string msgroupid;
		public int nextstageid;
		public int boxid;
		public int hpincreaserate;
		public int dmgincreaserate;
		public int islaststage;
	}
	public StageInfo[] StageinfoArray { get; private set; }
	public Dictionary<int, StageInfo> StageinfoDic { get; private set; }
	public void BindStageInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(StageinfoArray)).SetValue(this, deserializedData, null);
		StageinfoDic = StageinfoArray.ToDictionary(i => i.id);
	}
	public StageInfo GetStageInfoData(int _id) {
		if (StageinfoDic.TryGetValue(_id, out StageInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class StageMSGroup {
		public int id;
		public int groupid;
		public int unitid;
		public int unitcount;
	}
	public StageMSGroup[] StagemsgroupArray { get; private set; }
	public Dictionary<int, StageMSGroup> StagemsgroupDic { get; private set; }
	public void BindStageMSGroupData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(StagemsgroupArray)).SetValue(this, deserializedData, null);
		StagemsgroupDic = StagemsgroupArray.ToDictionary(i => i.id);
	}
	public StageMSGroup GetStageMSGroupData(int _id) {
		if (StagemsgroupDic.TryGetValue(_id, out StageMSGroup value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class UnitActionInfo {
		public int id;
		public int groupid;
		public string memo;
		public int actionorder;
		public int delay;
		public ACTION_TYPE actiontype;
		public int actionid;
		public int actionrepeattime;
		public int attacktype;
		public int attackrange;
		public string animname;
		public int attackinterrupt;
		public int knockavoidance;
	}
	public UnitActionInfo[] UnitactioninfoArray { get; private set; }
	public Dictionary<int, UnitActionInfo> UnitactioninfoDic { get; private set; }
	public void BindUnitActionInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(UnitactioninfoArray)).SetValue(this, deserializedData, null);
		UnitactioninfoDic = UnitactioninfoArray.ToDictionary(i => i.id);
	}
	public UnitActionInfo GetUnitActionInfoData(int _id) {
		if (UnitactioninfoDic.TryGetValue(_id, out UnitActionInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class UnitInfo {
		public int id;
		public string memo;
		public string name;
		public string icon;
		public string prefabname;
		public int hp;
		public int damage;
		public int movespeed;
		public int dropgroup;
		public int idleactiongroupid;
		public string combatactiongroupid;
		public string combatactiongrouprate;
		public int isboss;
		public int killexp;
	}
	public UnitInfo[] UnitinfoArray { get; private set; }
	public Dictionary<int, UnitInfo> UnitinfoDic { get; private set; }
	public void BindUnitInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(UnitinfoArray)).SetValue(this, deserializedData, null);
		UnitinfoDic = UnitinfoArray.ToDictionary(i => i.id);
	}
	public UnitInfo GetUnitInfoData(int _id) {
		if (UnitinfoDic.TryGetValue(_id, out UnitInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class UnitMoveInfo {
		public int id;
		public MOVE_TYPE movetype;
		public int movetypeparam;
		public int movevalue;
		public int movespeedrate;
	}
	public UnitMoveInfo[] UnitmoveinfoArray { get; private set; }
	public Dictionary<int, UnitMoveInfo> UnitmoveinfoDic { get; private set; }
	public void BindUnitMoveInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(UnitmoveinfoArray)).SetValue(this, deserializedData, null);
		UnitmoveinfoDic = UnitmoveinfoArray.ToDictionary(i => i.id);
	}
	public UnitMoveInfo GetUnitMoveInfoData(int _id) {
		if (UnitmoveinfoDic.TryGetValue(_id, out UnitMoveInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class UnitProjectileInfo {
		public int id;
		public string memo3;
		public string memo;
		public string prefabname;
		public int istargetturn;
		public int isdestroyedbywall;
		public int ismeleeatk;
		public int atkrate;
		public int fixedfirespeed;
		public int firespeed;
		public int trajectorytype;
		public int trajectoryparam1;
		public int trajectoryparam2;
		public int firetime;
		public int fireterm;
		public int fireangletype;
		public int fireangle;
		public int destroytime;
		public int showroutetime;
		public int knockback;
		public int airborne;
		public int knockbackduration;
		public int follow;
		public int wallbouncecnt;
		public int nextprojectileid;
		public int dotdmgid;
		public int pierce;
	}
	public UnitProjectileInfo[] UnitprojectileinfoArray { get; private set; }
	public Dictionary<int, UnitProjectileInfo> UnitprojectileinfoDic { get; private set; }
	public void BindUnitProjectileInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(UnitprojectileinfoArray)).SetValue(this, deserializedData, null);
		UnitprojectileinfoDic = UnitprojectileinfoArray.ToDictionary(i => i.id);
	}
	public UnitProjectileInfo GetUnitProjectileInfoData(int _id) {
		if (UnitprojectileinfoDic.TryGetValue(_id, out UnitProjectileInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class UnitTackleInfo {
		public int id;
		public int tackletype;
		public int tacklespeed;
		public int brakedist;
	}
	public UnitTackleInfo[] UnittackleinfoArray { get; private set; }
	public Dictionary<int, UnitTackleInfo> UnittackleinfoDic { get; private set; }
	public void BindUnitTackleInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(UnittackleinfoArray)).SetValue(this, deserializedData, null);
		UnittackleinfoDic = UnittackleinfoArray.ToDictionary(i => i.id);
	}
	public UnitTackleInfo GetUnitTackleInfoData(int _id) {
		if (UnittackleinfoDic.TryGetValue(_id, out UnitTackleInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class EquipmentInfo {
		public int id;
		public int grade;
		public string icon;
		public string memo;
		public string memo2;
		public string prefabname;
		public EQUIPMENT_TYPE equipmenttype;
		public int effectid;
		public int weapontype;
		public int projectileid;
		public int atkspeed;
		public int atkrange;
		public int dashrange;
		public int mergeid;
		public ATTRIBUTE_TYPE attributetype;
		public int attributevalue;
		public string attributedesc;
	}
	public EquipmentInfo[] EquipmentinfoArray { get; private set; }
	public Dictionary<int, EquipmentInfo> EquipmentinfoDic { get; private set; }
	public void BindEquipmentInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(EquipmentinfoArray)).SetValue(this, deserializedData, null);
		EquipmentinfoDic = EquipmentinfoArray.ToDictionary(i => i.id);
	}
	public EquipmentInfo GetEquipmentInfoData(int _id) {
		if (EquipmentinfoDic.TryGetValue(_id, out EquipmentInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class WeaponAnimInfo {
		public int id;
		public int weapongroup;
		public int order;
		public string fmemo;
		public string animname;
		public int projectileid;
		public int cooltime;
	}
	public WeaponAnimInfo[] WeaponaniminfoArray { get; private set; }
	public Dictionary<int, WeaponAnimInfo> WeaponaniminfoDic { get; private set; }
	public void BindWeaponAnimInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(WeaponaniminfoArray)).SetValue(this, deserializedData, null);
		WeaponaniminfoDic = WeaponaniminfoArray.ToDictionary(i => i.id);
	}
	public WeaponAnimInfo GetWeaponAnimInfoData(int _id) {
		if (WeaponaniminfoDic.TryGetValue(_id, out WeaponAnimInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class EquipmentLevelInfo {
		public int id;
		public int group;
		public string memo;
		public int level;
		public int attack;
		public int hp;
		public int costtype;
		public int costid;
		public int costcnt;
	}
	public EquipmentLevelInfo[] EquipmentlevelinfoArray { get; private set; }
	public Dictionary<int, EquipmentLevelInfo> EquipmentlevelinfoDic { get; private set; }
	public void BindEquipmentLevelInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(EquipmentlevelinfoArray)).SetValue(this, deserializedData, null);
		EquipmentlevelinfoDic = EquipmentlevelinfoArray.ToDictionary(i => i.id);
	}
	public EquipmentLevelInfo GetEquipmentLevelInfoData(int _id) {
		if (EquipmentlevelinfoDic.TryGetValue(_id, out EquipmentLevelInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class GachaInfo {
		public int id;
		public int gachagroupid;
		public int costtype;
		public int costid;
		public int costcnt;
	}
	public GachaInfo[] GachainfoArray { get; private set; }
	public Dictionary<int, GachaInfo> GachainfoDic { get; private set; }
	public void BindGachaInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(GachainfoArray)).SetValue(this, deserializedData, null);
		GachainfoDic = GachainfoArray.ToDictionary(i => i.id);
	}
	public GachaInfo GetGachaInfoData(int _id) {
		if (GachainfoDic.TryGetValue(_id, out GachaInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class GachaList {
		public int id;
		public int gachagroupdid;
		public int itemid;
		public int weight;
	}
	public GachaList[] GachalistArray { get; private set; }
	public Dictionary<int, GachaList> GachalistDic { get; private set; }
	public void BindGachaListData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(GachalistArray)).SetValue(this, deserializedData, null);
		GachalistDic = GachalistArray.ToDictionary(i => i.id);
	}
	public GachaList GetGachaListData(int _id) {
		if (GachalistDic.TryGetValue(_id, out GachaList value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class DungeonItemInfo {
		public int id;
		public int grade;
		public string name;
		public string iconname;
		public int sellrewardtype;
		public int sellrewardid;
		public int sellrewardcnt;
	}
	public DungeonItemInfo[] DungeoniteminfoArray { get; private set; }
	public Dictionary<int, DungeonItemInfo> DungeoniteminfoDic { get; private set; }
	public void BindDungeonItemInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(DungeoniteminfoArray)).SetValue(this, deserializedData, null);
		DungeoniteminfoDic = DungeoniteminfoArray.ToDictionary(i => i.id);
	}
	public DungeonItemInfo GetDungeonItemInfoData(int _id) {
		if (DungeoniteminfoDic.TryGetValue(_id, out DungeonItemInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class DropInfo {
		public int id;
		public int dropgroup;
		public ITEM_TYPE rewardtype;
		public int rewardid;
		public int rewardcnt;
		public int rate;
	}
	public DropInfo[] DropinfoArray { get; private set; }
	public Dictionary<int, DropInfo> DropinfoDic { get; private set; }
	public void BindDropInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(DropinfoArray)).SetValue(this, deserializedData, null);
		DropinfoDic = DropinfoArray.ToDictionary(i => i.id);
	}
	public DropInfo GetDropInfoData(int _id) {
		if (DropinfoDic.TryGetValue(_id, out DropInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class DungeonBoxInfo {
		public int id;
		public int dropgroup;
		public int rate;
	}
	public DungeonBoxInfo[] DungeonboxinfoArray { get; private set; }
	public Dictionary<int, DungeonBoxInfo> DungeonboxinfoDic { get; private set; }
	public void BindDungeonBoxInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(DungeonboxinfoArray)).SetValue(this, deserializedData, null);
		DungeonboxinfoDic = DungeonboxinfoArray.ToDictionary(i => i.id);
	}
	public DungeonBoxInfo GetDungeonBoxInfoData(int _id) {
		if (DungeonboxinfoDic.TryGetValue(_id, out DungeonBoxInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class ItemInfo {
		public int id;
		public ITEM_TYPE itemtype;
		public string itemname;
		public string itemdesc;
		public string iconname;
	}
	public ItemInfo[] IteminfoArray { get; private set; }
	public Dictionary<int, ItemInfo> IteminfoDic { get; private set; }
	public void BindItemInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(IteminfoArray)).SetValue(this, deserializedData, null);
		IteminfoDic = IteminfoArray.ToDictionary(i => i.id);
	}
	public ItemInfo GetItemInfoData(int _id) {
		if (IteminfoDic.TryGetValue(_id, out ItemInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class DungeonCharacterLevel {
		public int id;
		public int exp;
	}
	public DungeonCharacterLevel[] DungeoncharacterlevelArray { get; private set; }
	public Dictionary<int, DungeonCharacterLevel> DungeoncharacterlevelDic { get; private set; }
	public void BindDungeonCharacterLevelData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(DungeoncharacterlevelArray)).SetValue(this, deserializedData, null);
		DungeoncharacterlevelDic = DungeoncharacterlevelArray.ToDictionary(i => i.id);
	}
	public DungeonCharacterLevel GetDungeonCharacterLevelData(int _id) {
		if (DungeoncharacterlevelDic.TryGetValue(_id, out DungeonCharacterLevel value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class CharacterInfo {
		public int id;
		public string icon;
		public string prefabname;
		public string name;
		public int damage;
		public int movespeed;
		public int hp;
		public int defense;
	}
	public CharacterInfo[] CharacterinfoArray { get; private set; }
	public Dictionary<int, CharacterInfo> CharacterinfoDic { get; private set; }
	public void BindCharacterInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(CharacterinfoArray)).SetValue(this, deserializedData, null);
		CharacterinfoDic = CharacterinfoArray.ToDictionary(i => i.id);
	}
	public CharacterInfo GetCharacterInfoData(int _id) {
		if (CharacterinfoDic.TryGetValue(_id, out CharacterInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class AbilityInfo {
		public int id;
		public string name;
		public string desc;
		public string icon;
		public int weight;
		public int maxskillcount;
		public int weapontype;
	}
	public AbilityInfo[] AbilityinfoArray { get; private set; }
	public Dictionary<int, AbilityInfo> AbilityinfoDic { get; private set; }
	public void BindAbilityInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(AbilityinfoArray)).SetValue(this, deserializedData, null);
		AbilityinfoDic = AbilityinfoArray.ToDictionary(i => i.id);
	}
	public AbilityInfo GetAbilityInfoData(int _id) {
		if (AbilityinfoDic.TryGetValue(_id, out AbilityInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class AbilityEffectInfo {
		public int id;
		public int effectgroup;
		public string memo;
		public ABILITY_CONDITION_TYPE conditiontype;
		public int conditionvalue1;
		public int conditionvalue2;
		public ATTRIBUTE_TYPE attributetype;
		public int attributevalue;
		public int attributevalue2;
	}
	public AbilityEffectInfo[] AbilityeffectinfoArray { get; private set; }
	public Dictionary<int, AbilityEffectInfo> AbilityeffectinfoDic { get; private set; }
	public void BindAbilityEffectInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(AbilityeffectinfoArray)).SetValue(this, deserializedData, null);
		AbilityeffectinfoDic = AbilityeffectinfoArray.ToDictionary(i => i.id);
	}
	public AbilityEffectInfo GetAbilityEffectInfoData(int _id) {
		if (AbilityeffectinfoDic.TryGetValue(_id, out AbilityEffectInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class GuideMissionInfo {
		public int id;
		public int missiontype;
		public int missiontarget;
		public int missionvalue;
		public string missiondesc;
		public int rewardtype;
		public int rewardid;
		public int rewardcnt;
	}
	public GuideMissionInfo[] GuidemissioninfoArray { get; private set; }
	public Dictionary<int, GuideMissionInfo> GuidemissioninfoDic { get; private set; }
	public void BindGuideMissionInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(GuidemissioninfoArray)).SetValue(this, deserializedData, null);
		GuidemissioninfoDic = GuidemissioninfoArray.ToDictionary(i => i.id);
	}
	public GuideMissionInfo GetGuideMissionInfoData(int _id) {
		if (GuidemissioninfoDic.TryGetValue(_id, out GuideMissionInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class DungeonEventList {
		public int id;
		public int floorid;
		public int conditiontype;
		public int conditionvalue;
		public DUNGEON_EVENT_TYPE eventtype;
		public int weight;
		public int dialoggroup;
	}
	public DungeonEventList[] DungeoneventlistArray { get; private set; }
	public Dictionary<int, DungeonEventList> DungeoneventlistDic { get; private set; }
	public void BindDungeonEventListData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(DungeoneventlistArray)).SetValue(this, deserializedData, null);
		DungeoneventlistDic = DungeoneventlistArray.ToDictionary(i => i.id);
	}
	public DungeonEventList GetDungeonEventListData(int _id) {
		if (DungeoneventlistDic.TryGetValue(_id, out DungeonEventList value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class DungeonEventDialog {
		public int id;
		public DUNGEON_EVENT_TYPE eventtype;
		public int dialoggroup;
		public string startdesc;
		public string positiveresult;
		public string negativeresult;
		public int weight;
	}
	public DungeonEventDialog[] DungeoneventdialogArray { get; private set; }
	public Dictionary<int, DungeonEventDialog> DungeoneventdialogDic { get; private set; }
	public void BindDungeonEventDialogData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(DungeoneventdialogArray)).SetValue(this, deserializedData, null);
		DungeoneventdialogDic = DungeoneventdialogArray.ToDictionary(i => i.id);
	}
	public DungeonEventDialog GetDungeonEventDialogData(int _id) {
		if (DungeoneventdialogDic.TryGetValue(_id, out DungeonEventDialog value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class DungeonEventBattle {
		public int id;
		public int reqfloor;
		public int stageid;
		public int dialoggroup;
	}
	public DungeonEventBattle[] DungeoneventbattleArray { get; private set; }
	public Dictionary<int, DungeonEventBattle> DungeoneventbattleDic { get; private set; }
	public void BindDungeonEventBattleData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(DungeoneventbattleArray)).SetValue(this, deserializedData, null);
		DungeoneventbattleDic = DungeoneventbattleArray.ToDictionary(i => i.id);
	}
	public DungeonEventBattle GetDungeonEventBattleData(int _id) {
		if (DungeoneventbattleDic.TryGetValue(_id, out DungeonEventBattle value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class DungeonEventBuff {
		public int id;
		public ATTRIBUTE_TYPE attributetype;
		public int attributevalue;
		public string attributedesc;
		public int weight;
		public int dialoggroup;
	}
	public DungeonEventBuff[] DungeoneventbuffArray { get; private set; }
	public Dictionary<int, DungeonEventBuff> DungeoneventbuffDic { get; private set; }
	public void BindDungeonEventBuffData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(DungeoneventbuffArray)).SetValue(this, deserializedData, null);
		DungeoneventbuffDic = DungeoneventbuffArray.ToDictionary(i => i.id);
	}
	public DungeonEventBuff GetDungeonEventBuffData(int _id) {
		if (DungeoneventbuffDic.TryGetValue(_id, out DungeonEventBuff value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class DotDmgInfo {
		public int id;
		public int type;
		public int atk;
		public int time;
		public string effectname;
	}
	public DotDmgInfo[] DotdmginfoArray { get; private set; }
	public Dictionary<int, DotDmgInfo> DotdmginfoDic { get; private set; }
	public void BindDotDmgInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(DotdmginfoArray)).SetValue(this, deserializedData, null);
		DotdmginfoDic = DotdmginfoArray.ToDictionary(i => i.id);
	}
	public DotDmgInfo GetDotDmgInfoData(int _id) {
		if (DotdmginfoDic.TryGetValue(_id, out DotDmgInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class CustomerInfo {
		public int id;
		public int order;
		public int waitday;
		public int itemtype1;
		public int itemid1;
		public int itemcnt1;
		public int itemtype2;
		public int itemid2;
		public int itemcnt2;
		public int itemtype3;
		public int itemid3;
		public int itemcnt3;
		public int itemtype4;
		public int itemid4;
		public int itemcnt4;
		public int rewardtype1;
		public int rewardid1;
		public int rewardcnt1;
		public int rewardtype2;
		public int rewardid2;
		public int rewardcnt2;
	}
	public CustomerInfo[] CustomerinfoArray { get; private set; }
	public Dictionary<int, CustomerInfo> CustomerinfoDic { get; private set; }
	public void BindCustomerInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(CustomerinfoArray)).SetValue(this, deserializedData, null);
		CustomerinfoDic = CustomerinfoArray.ToDictionary(i => i.id);
	}
	public CustomerInfo GetCustomerInfoData(int _id) {
		if (CustomerinfoDic.TryGetValue(_id, out CustomerInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class ShopReputationLevel {
		public int id;
		public int level;
		public int reputationscore;
		public int rewardtype1;
		public int rewardid1;
		public int rewardcnt1;
		public int rewardtype2;
		public int rewardid2;
		public int rewardcnt2;
		public int rewardtype3;
		public int rewardid3;
		public int rewardcnt3;
		public int rewardtype4;
		public int rewardid4;
		public int rewardcnt4;
		public int rewardtype5;
		public int rewardid5;
		public int rewardcnt5;
	}
	public ShopReputationLevel[] ShopreputationlevelArray { get; private set; }
	public Dictionary<int, ShopReputationLevel> ShopreputationlevelDic { get; private set; }
	public void BindShopReputationLevelData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(ShopreputationlevelArray)).SetValue(this, deserializedData, null);
		ShopreputationlevelDic = ShopreputationlevelArray.ToDictionary(i => i.id);
	}
	public ShopReputationLevel GetShopReputationLevelData(int _id) {
		if (ShopreputationlevelDic.TryGetValue(_id, out ShopReputationLevel value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class SummonInfo {
		public int id;
		public string prefabname;
		public string memo;
		public int projectileid;
		public int atkrate;
		public int delay;
	}
	public SummonInfo[] SummoninfoArray { get; private set; }
	public Dictionary<int, SummonInfo> SummoninfoDic { get; private set; }
	public void BindSummonInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(SummoninfoArray)).SetValue(this, deserializedData, null);
		SummoninfoDic = SummoninfoArray.ToDictionary(i => i.id);
	}
	public SummonInfo GetSummonInfoData(int _id) {
		if (SummoninfoDic.TryGetValue(_id, out SummonInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class ObjectInfo {
		public int id;
		public OBJECT_TYPE objecttype;
		public string prefabname;
		public int atk;
		public int hp;
		public int dropgroup;
		public int projectileid;
	}
	public ObjectInfo[] ObjectinfoArray { get; private set; }
	public Dictionary<int, ObjectInfo> ObjectinfoDic { get; private set; }
	public void BindObjectInfoData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(ObjectinfoArray)).SetValue(this, deserializedData, null);
		ObjectinfoDic = ObjectinfoArray.ToDictionary(i => i.id);
	}
	public ObjectInfo GetObjectInfoData(int _id) {
		if (ObjectinfoDic.TryGetValue(_id, out ObjectInfo value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
	public partial class ShopUpgrade {
		public int id;
		public int type;
		public string iconname;
		public ATTRIBUTE_TYPE attributetype;
		public int attributevalue;
		public int costtype;
		public int costid;
		public int costcnt;
	}
	public ShopUpgrade[] ShopupgradeArray { get; private set; }
	public Dictionary<int, ShopUpgrade> ShopupgradeDic { get; private set; }
	public void BindShopUpgradeData(Type type, string text) {
		var deserializedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(ShopupgradeArray)).SetValue(this, deserializedData, null);
		ShopupgradeDic = ShopupgradeArray.ToDictionary(i => i.id);
	}
	public ShopUpgrade GetShopUpgradeData(int _id) {
		if (ShopupgradeDic.TryGetValue(_id, out ShopUpgrade value)) {
			return value;
		}
		Debug.LogError($"테이블에 ID가 없습니다: {_id}");
		return null;
	}
}
