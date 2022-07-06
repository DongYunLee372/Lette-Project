using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestAssets_Bundle : MonoBehaviour
{
	/***********************************************************************
		 * �뵵 : MenuItem�� ����ϸ� �޴�â�� ���ο� �޴��� �߰��� �� �ֽ��ϴ�.		      
		 * (�Ʒ��� �ڵ忡���� Bundles �׸� ���� �׸����� Build AssetBundles �׸��� �߰�.)  
		 ***********************************************************************/
	[MenuItem("Bundles/Build AssetBundles")]
	static void BuildAllAssetBundles()
	{
		/***********************************************************************
		* �̸� : BuildPipeLine.BuildAssetBundles()
	    * �뵵 : BuildPipeLine Ŭ������ �Լ� BuildAssetBundles()�� ���¹����� ������ݴϴ�.     
	    * �Ű��������� String ���� �ѱ�� �Ǹ�, ����� ���� ������ ������ ����Դϴ�. 
	    * ���� ��� Assets ���� ������ �����Ϸ��� "Assets/AssetBundles"�� �Է��ؾ��մϴ�.
	    ***********************************************************************/
		BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

	}
	

}
