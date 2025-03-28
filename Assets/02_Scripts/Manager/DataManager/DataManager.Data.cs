#pragma warning disable 114
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public partial class DataManager {
	public partial class Localization {
		public string id;
		public string ko;
		public string en;
		public string jp;
	};
	public Localization[] LocalizationArray { get; private set; }
	public Dictionary<string, Localization> LocalizationDic { get; private set; }
	public void BindLocalizationData(Type type, string text){
		var deserializaedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(LocalizationArray)).SetValue(this, deserializaedData, null);
		LocalizationDic = LocalizationArray.ToDictionary(i => i.id);
	}
	public Localization GetLocalizationData(string _id){
		if (LocalizationDic.TryGetValue(_id, out Localization value)){
			return value;
		}
		UnityEngine.Debug.LogError($"table doesnt contain id {_id}");
		return null;
	}
	public partial class Level {
		public int id;
		public int level;
		public int exp;
		public int unlockslot;
		public int goldreward;
	};
	public Level[] LevelArray { get; private set; }
	public Dictionary<int, Level> LevelDic { get; private set; }
	public void BindLevelData(Type type, string text){
		var deserializaedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(LevelArray)).SetValue(this, deserializaedData, null);
		LevelDic = LevelArray.ToDictionary(i => i.id);
	}
	public Level GetLevelData(int _id){
		if (LevelDic.TryGetValue(_id, out Level value)){
			return value;
		}
		UnityEngine.Debug.LogError($"table doesnt contain id {_id}");
		return null;
	}
	public partial class GridStageInfo {
		public int id;
		public string prefabname;
		public int msconditiontype1;
		public int msconditiontype2;
		public int msconditiontype3;
		public int msconditiontype4;
		public int msconditionvalue1;
		public int msconditionvalue2;
		public int msconditionvalue3;
		public int msconditionvalue4;
		public int msgroupid1;
		public int msgroupid2;
		public int msgroupid3;
		public int msgroupid4;
		public int nextstageid;
	};
	public GridStageInfo[] GridstageinfoArray { get; private set; }
	public Dictionary<int, GridStageInfo> GridstageinfoDic { get; private set; }
	public void BindGridStageInfoData(Type type, string text){
		var deserializaedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(GridstageinfoArray)).SetValue(this, deserializaedData, null);
		GridstageinfoDic = GridstageinfoArray.ToDictionary(i => i.id);
	}
	public GridStageInfo GetGridStageInfoData(int _id){
		if (GridstageinfoDic.TryGetValue(_id, out GridStageInfo value)){
			return value;
		}
		UnityEngine.Debug.LogError($"table doesnt contain id {_id}");
		return null;
	}
	public partial class GridStageMSGroup {
		public int id;
		public int groupid;
		public int unitid;
		public int unitcount;
	};
	public GridStageMSGroup[] GridstagemsgroupArray { get; private set; }
	public Dictionary<int, GridStageMSGroup> GridstagemsgroupDic { get; private set; }
	public void BindGridStageMSGroupData(Type type, string text){
		var deserializaedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(GridstagemsgroupArray)).SetValue(this, deserializaedData, null);
		GridstagemsgroupDic = GridstagemsgroupArray.ToDictionary(i => i.id);
	}
	public GridStageMSGroup GetGridStageMSGroupData(int _id){
		if (GridstagemsgroupDic.TryGetValue(_id, out GridStageMSGroup value)){
			return value;
		}
		UnityEngine.Debug.LogError($"table doesnt contain id {_id}");
		return null;
	}
};
