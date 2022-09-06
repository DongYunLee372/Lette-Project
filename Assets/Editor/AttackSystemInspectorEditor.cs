using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


[CustomEditor(typeof(CAttackComponent))]
public class AttackSystemInspectorEditor : Editor
{
    CAttackComponent _attack;
    ReorderableList _list;

    //private void OnEnable()
    //{
    //    _attack = target as CAttackComponent;
    //    _list = new ReorderableList(serializedObject, serializedObject.FindProperty("attackinfoList"), true, true, true, true);

    //    _list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
    //    {
    //        //화면에 그려지는 아이템 리스트를 얻는 부분
    //        var element = _list.serializedProperty.GetArrayElementAtIndex(index);
    //        //SerializedProperty aa;
    //        rect.y += 2;
    //        int i = 0;

    //        _list.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "AttackInfoList"); };
    //        _list.elementHeight = 350;

    //        //EditorGUI.PropertyField(new Rect(rect.x, rect.y -25 , 450, EditorGUIUtility.singleLineHeight), element.GetArrayElementAtIndex(1), new GUIContent($"ss"));
    //        int type = element.FindPropertyRelative("AttackType").enumValueIndex;
            
    //        foreach (SerializedProperty a in element)
    //        {
    //            if (type == (int)CharEnumTypes.eAttackType.Normal)
    //            {
    //                if(a.displayName == "Projectile Obj" || a.displayName == "Target Obj")
    //                    continue;
    //            }
    //            else if(type == (int)CharEnumTypes.eAttackType.Projectile)
    //            {
    //                if (a.displayName == "Target Obj")
    //                    continue;
    //            }
    //            else if (type == (int)CharEnumTypes.eAttackType.AreaOfEffect)
    //            {
    //                if (a.displayName == "Projectile Obj" || a.displayName == "Target Obj")
    //                    continue;
    //            }
    //            else if (type == (int)CharEnumTypes.eAttackType.Targeting)
    //            {
    //                if (a.displayName == "Projectile Obj")
    //                    continue;
    //            }

    //            EditorGUI.PropertyField(new Rect(rect.x, rect.y + i*25, 450, EditorGUIUtility.singleLineHeight), a, new GUIContent($"{a.displayName}"));
    //            i++;
    //        }

    //        //FindPropertyRelative 메소드를 이용해서 웨이브의 속성을 찾을 수 있다.
    //        //EditorGUI.PropertyField(new Rect(rect.x, rect.y, 450, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AttackType"), new GUIContent("AttackType"));
    //    };

        
    //}

    //public override void OnInspectorGUI()
    //{
    //    _attack.CurAttackNum = EditorGUILayout.IntField(new GUIContent("CurAttackNum", "현재 사용중인 공격의 번호(연속공격일때만 사용)"), _attack.CurAttackNum);
    //    UseProperty("attackinfos");

    //    serializedObject.Update();
    //    _list.DoLayoutList();
    //    serializedObject.ApplyModifiedProperties();

    //}

    //void UseProperty(string propertyName)   //해당 변수를 원래의 pubilc 형태로 사용    
    //{   //배열의 경우 이곳으로 불러오는 기능을 자체적으로 지원하지 않는다. 여러 방법이 있겠지만 이 방법을 쓰면 원래 쓰던 그대로를 가져올 수 있다.        
    //    SerializedProperty tps = serializedObject.FindProperty(propertyName);   

    //    //변수명을 입력해서 찾는다.       
    //    EditorGUI.BeginChangeCheck();   
    //    //Begin과 End로 값이 바뀌는 것을 검사한다.        
    //    EditorGUILayout.PropertyField(tps, true);//변수에 맞는 필드 생성. 인자의 true부분은 includeChildren로써 자식에 해당하는 부분까지 모두 가져온다는 뜻이다.                                                    
    //    //만약 여기서 false를 하면 변수명 자체는 인스펙터 창에 뜨지만 배열항목이 아예 뜨지 않아 이름뿐인 항목이 된다.
                
    //    if (EditorGUI.EndChangeCheck()) //여기까지 검사해서 필드에 변화가 있으면            
    //    serializedObject.ApplyModifiedProperties(); //원래 변수에 적용시킨다.         
    //    //툴팁의 경우 원래 스크립트의 있는 것을 가져온다.    
    //}

}
