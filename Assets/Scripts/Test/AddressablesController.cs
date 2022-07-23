using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class AddressablesController : MonoBehaviour
{
	[SerializeField]
	private string _label;
	bool flag = true;
	[SerializeField]
//	private List<GameObject> _createdObjs { get; } = new List<GameObject>();
	private List<GameObject> _createdObjs = new List<GameObject>();
	GameObject tempob;

	private void Start()
	{

		//Instantiate("test1");
		Instantiate("Monster");

		temp_Show_list();
	}

	private async void Instantiate(string label)
	{
		
		await AddressablesLoader.InitAssets_label(label, _createdObjs);
		//setPos();

	
		//temp_Show_list();

	}

	public void testLoadAsset()
    {
		string name = "susu";
		//GameObject obj=null;
		StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial(name));
		

		foreach(var obj in AddressablesLoader.tempobj)
		{
			if(name==obj.name)
            {
				Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity);
				Debug.Log(obj.name + "����Ʈ����");
			}
		
		}
	}



	void temp_Show_list()
	{
		Debug.Log("�̸�����");
		int c=0;

		foreach (var obj in _createdObjs)
		{
			c++;
			Debug.Log("��������"+obj.name);
		}
		Debug.Log(c);

	}

	void setPos()
	{
		foreach (var obj in _createdObjs)
		{
			obj.transform.position = new Vector3(0, 0, 0);
		
		}

	}

	//�̸����� ������ addAsset���ֱ�
	public async void addAsset(string name)
	{
		await AddressablesLoader.InitAssets_name(name, _createdObjs);

	}

	

	private void Update()
    {
		if (AddressablesLoader.tempobj!=null)
        {

        }

		//if(_createdObjs!=null)
  //      {
		//	foreach (var obj in _createdObjs)
		//	{
		//		Debug.Log("��������" + obj.name);
		//	}
		//}
		
	}

	//���� �ø��ſ��� prefabsName ��ġ�ϴ� ������ ���� ��ȯ.
	public GameObject AddClone(string prefabsName)
	{
		GameObject tempGameobj = null;

		foreach (var obj in AddressablesLoader.tempobj)
		{
			if (obj.name == prefabsName)
			{
				tempGameobj = Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity);
				flag = false;
				break;
			}
		}
		return tempGameobj;
	}


    //�̸����� ������ addAsset���ֱ�
    public async void LoadResource(string name)
    {
        await AddressablesLoader.InitAssets_name(name);
    }

    ////��巹���� �̸�,�������̸�
    //public async void LoadResource(string name, string prefab)
    //{
    //	await AddressablesLoader.InitAssets_name(name);
    //	tempob = AddClone(prefab);
    //}

    public  GameObject Get_LoadResource(string name,string prefabsname)
    {
		LoadResource(name);

		foreach (var obj in AddressablesLoader.tempobj)
        {
			//if (prefabsname == obj.name)
   //         {
			//	Debug.Log(obj.name + "�ε��ؿ�");

			//	return obj;
   //         }

			Debug.Log(obj.name);
		}
		Debug.Log("��ġ�ϴ� ������ ���� null��ȯ");
		return null;
	}


	//���� ����Ʈ�� �޾ư���?  ����? ->Ǯ�� �ϰ�������
	//public GameObject AddClone<T>(string prefabsName,List<GameObject> saveCloneList)
	//{
	//	GameObject tempGameobj = null;

	//	foreach (var obj in AddressablesLoader.tempobj)
	//	{
	//		if (obj.name == prefabsName)
	//		{
	//			saveCloneList.Add(Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity));
	//			flag = false;
	//			break;
	//		}
	//	}
	//	return tempGameobj;
	//}


	//�������� ������ �� ����...?
	void add()
	{

		foreach (var obj in AddressablesLoader.tempobj)
		{
			tempob = Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity);
			flag = false;
		}
		//AddressablesLoader.tempobj.Clear();
	}


	//����Ʈ�� ���� �� �� ����..?
	public void tempdes()
	{
		foreach (var obj in AddressablesLoader.tempobj)
		{
			GameObject tem = obj;
			Destroy_Obj(ref tem, tempob);
			break;
		}
	}

	public GameObject LoadAsset(string name)
    {
		GameObject temp =AddressablesLoader.returnAssets(name);
		_createdObjs.Add(temp);
		return temp;
	}

	public void Destroy_Obj(ref GameObject deleteMemory, GameObject deleteobj)  //�޸� ���� �� ������Ʈ,������ ������Ʈ.
	{
		if (!Addressables.ReleaseInstance(deleteMemory))
		{
			Destroy(deleteobj);
			Addressables.ReleaseInstance(deleteMemory);
			AddressablesLoader.tempobj.Remove(deleteMemory);
			Debug.Log("��ü �޸� ����");
		}
	}

	//������ -> �޸� ���� �Ͻñ� ���� �޸𸮸� ����ϴ� ������Ʈ���� ���� destroy �ϰ� �Լ� ȣ�����ּ���!
	public void Destroy_Obj(ref GameObject deleteMemory)  //�޸� ���� �� ������Ʈ.
	{
		if (!Addressables.ReleaseInstance(deleteMemory))
		{
			Addressables.ReleaseInstance(deleteMemory);
			AddressablesLoader.tempobj.Remove(deleteMemory);
			Debug.Log("�޸� ����");
		}
	}
	//���̺� ����
	public void Destroy_Obj(GameObject obj)  //������ ������Ʈ
	{
		if (!Addressables.ReleaseInstance(obj))
		{
			Addressables.ReleaseInstance(obj);
			Destroy(obj);
			_createdObjs.Remove(obj);
		}
	}

}