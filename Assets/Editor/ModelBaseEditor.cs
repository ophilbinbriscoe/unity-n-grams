using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.IMGUI.Controls;
using NGram;

[CustomEditor( typeof( ModelEditorBase ), true, isFallback = false )]
public class ModelEditor : Editor
{
	SerializedProperty train, nGrams;

	private void OnEnable ()
	{
		train = serializedObject.FindProperty( "train" );
		nGrams = serializedObject.FindProperty( "nGrams" );
	}
	
	public override void OnInspectorGUI ()
	{
		serializedObject.Update();
		
		EditorGUILayout.Space();
		
		DrawPropertiesExcluding( serializedObject, "m_Script", "train", "nGrams" );
		
		if ( GUILayout.Button( "Train" ) )
		{
			train.boolValue = true;
		}

		//using ( new EditorGUI.DisabledScope( true ) )
		{
			EditorGUILayout.PropertyField( nGrams );
		}

		serializedObject.ApplyModifiedProperties();
	}
}
