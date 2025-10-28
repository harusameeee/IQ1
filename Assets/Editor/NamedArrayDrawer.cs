using UnityEditor;
using UnityEngine;

/// <summary>
/// NamedArrayAttribute に応じて配列要素や単体フィールドのラベルを変更するDrawer
/// </summary>
[CustomPropertyDrawer(typeof(NamedArrayAttribute))]
public class NamedArrayDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var namedArray = (NamedArrayAttribute)attribute;

        // --- 配列の要素かどうかを判断 ---
        int index = GetElementIndex(property.propertyPath);

        // 配列要素として使われている場合
        if (index >= 0 && namedArray.DisplayNames != null && index < namedArray.DisplayNames.Length)
        {
            label.text = namedArray.DisplayNames[index];
        }
        // 単体フィールドとして使われている場合
        else if (!string.IsNullOrEmpty(namedArray.DisplayName))
        {
            label.text = namedArray.DisplayName;
        }

        EditorGUI.PropertyField(position, property, label, true);
    }

    /// <summary>
    /// propertyPath から [0] のようなインデックスを抽出
    /// </summary>
    private int GetElementIndex(string propertyPath)
    {
        int start = propertyPath.IndexOf('[') + 1;
        int end = propertyPath.IndexOf(']');
        if (start > 0 && end > start)
        {
            string num = propertyPath.Substring(start, end - start);
            if (int.TryParse(num, out int index))
                return index;
        }
        return -1;
    }
}
