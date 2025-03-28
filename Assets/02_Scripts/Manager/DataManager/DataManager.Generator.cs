using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Linq;

public partial class DataManager
{
    private static readonly string DATATABLE_DEF_PATH = "Assets/02_Scripts/Manager/DataManager/DataManager.Data.cs";
    private static readonly string CONFIG_TABLE_DEF_PATH = "Assets/02_Scripts/Manager/DataManager/ConfigTable.cs";
    private static readonly string TABLE_ENUM_DEF_PATH = "Assets/02_Scripts/Manager/DataManager/EnumTable.cs";
    private static readonly string LOCAL_CSV_PATH = $"{Application.dataPath}/Resources/Data";
    private static readonly string CONFIG_TABLE_NAME = "ConfigTable.csv";
    private static readonly string ENUM_TABLE_NAME = "EnumTable.csv";

    public static void GenDatatable()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#pragma warning disable 114");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine();
            sb.AppendLine("public partial class DataManager {");

            GenTableData(sb);
            sb.AppendLine("}");

            WriteCode(DATATABLE_DEF_PATH, sb.ToString());
        }
        catch (Exception e)
        {
            Debug.LogError($"데이터 테이블 생성 실패: {e.Message}");
            throw;
        }
    }

    private static void GenTableData(StringBuilder sb)
    {
        foreach (var tableName in tableNames)
        {
            try
            {
                var data = File.ReadAllText(Path.Combine(LOCAL_CSV_PATH, $"{tableName}.csv"));
                var rows = CSVSerializer.ParseCSV(data, '|');

                string tableNameUpper = $"{tableName[0].ToString().ToUpper()}{tableName.Substring(1).ToLower()}";
                string arrayName = $"{tableNameUpper}Array";
                string dicName = $"{tableNameUpper}Dic";

                sb.AppendLine($"\tpublic partial class {tableName} {{");

                for (int i = 0; i < rows[0].Length; i++)
                {
                    var type = rows[1][i];
                    if (!(type == "int" || type == "long" || type == "float" || type == "string"))
                    {
                        type = type.ToUpper();
                    }
                    sb.AppendLine($"\t\tpublic {type} {rows[0][i].ToLower()};");
                }

                var keyType = rows[1][0];
                if (!(keyType == "int" || keyType == "long" || keyType == "float" || keyType == "string"))
                {
                    keyType = keyType.ToLower();
                }

                sb.AppendLine("\t}");
                sb.AppendLine($"\tpublic {tableName}[] {arrayName} {{ get; private set; }}");
                sb.AppendLine($"\tpublic Dictionary<{keyType}, {tableName}> {dicName} {{ get; private set; }}");

                sb.AppendLine($"\tpublic void Bind{tableName}Data(Type type, string text) {{");
                sb.AppendLine("\t\tvar deserializedData = CSVDeserialize(text, type);");
                sb.AppendLine($"\t\tGetType().GetProperty(nameof({arrayName})).SetValue(this, deserializedData, null);");
                sb.AppendLine($"\t\t{dicName} = {arrayName}.ToDictionary(i => i.id);");
                sb.AppendLine("\t}");

                sb.AppendLine($"\tpublic {tableName} Get{tableName}Data({keyType} _id) {{");
                sb.AppendLine($"\t\tif ({dicName}.TryGetValue(_id, out {tableName} value)) {{");
                sb.AppendLine("\t\t\treturn value;");
                sb.AppendLine("\t\t}");
                sb.AppendLine($"\t\tDebug.LogError($\"테이블에 ID가 없습니다: {{_id}}\");");
                sb.AppendLine("\t\treturn null;");
                sb.AppendLine("\t}");
            }
            catch (Exception e)
            {
                Debug.LogError($"테이블 생성 실패 {tableName}: {e.Message}");
                throw;
            }
        }
    }

    public static void GenConfigTable()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#pragma warning disable 114");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Reflection;");
            sb.AppendLine();
            sb.AppendLine("public class ConfigTable : Singleton<ConfigTable> {");

            GenConfigTableData(sb);
            sb.AppendLine("}");

            WriteCode(CONFIG_TABLE_DEF_PATH, sb.ToString());
        }
        catch (Exception e)
        {
            Debug.LogError($"설정 테이블 생성 실패: {e.Message}");
            throw;
        }
    }

    private static void GenConfigTableData(StringBuilder sb)
    {
        var data = File.ReadAllText(Path.Combine(LOCAL_CSV_PATH, CONFIG_TABLE_NAME));
        var rows = CSVSerializer.ParseCSV(data, '|');

        for (int i = 2; i < rows.Count; i++)
        {
            var name = rows[i][0];
            var type = rows[i][1];
            sb.AppendLine($"\tpublic {type} {name};");
        }

        sb.AppendLine("\tpublic void LoadConfig(Dictionary<string, Dictionary<string, object>> rowList)");
        sb.AppendLine("\t{");
        sb.AppendLine("\t\tforeach (var rowItem in rowList)");
        sb.AppendLine("\t\t{");
        sb.AppendLine("\t\t\tvar field = typeof(ConfigTable).GetField(rowItem.Key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);");
        sb.AppendLine("\t\t\tfield.SetValue(this, rowItem.Value[\"value\"]);");
        sb.AppendLine("\t\t}");
        sb.AppendLine("\t}");
    }

    public static void GenTableEnum()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            GenTableEnum(sb);
            WriteCode(TABLE_ENUM_DEF_PATH, sb.ToString());
        }
        catch (Exception e)
        {
            Debug.LogError($"열거형 테이블 생성 실패: {e.Message}");
            throw;
        }
    }

    private static void GenTableEnum(StringBuilder sb)
    {
        var data = File.ReadAllText(Path.Combine(LOCAL_CSV_PATH, ENUM_TABLE_NAME));
        var rows = CSVSerializer.ParseCSV(data, '|');

        HashSet<string> keySet = new HashSet<string>();

        for (int i = 2; i < rows.Count; i++)
        {
            string enumType = rows[i][0].ToUpper();
            if (!keySet.Contains(enumType))
            {
                if (keySet.Count > 0)
                    sb.AppendLine("}");
                sb.AppendLine($"public enum {enumType}");
                sb.AppendLine("{");
                keySet.Add(enumType);
            }

            sb.AppendLine($"\t{rows[i][1].ToUpper(),-28} = {rows[i][2],-10}, // {rows[i][3]}");
        }
        sb.AppendLine("}");
    }

    private static void WriteCode(string filePath, string content)
    {
        try
        {
            File.WriteAllText(filePath, content);
            Debug.Log($"파일 생성 완료: {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"파일 생성 실패 {filePath}: {e.Message}");
            throw;
        }
    }
}
