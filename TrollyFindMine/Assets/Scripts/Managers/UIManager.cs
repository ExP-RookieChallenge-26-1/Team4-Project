using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private readonly Dictionary<string, UIView> _uiDic = new Dictionary<string, UIView>();
    private readonly HashSet<UIView> _uiActivated = new HashSet<UIView>();
    private readonly Stack<UIViewStackable> _uiStack = new Stack<UIViewStackable>();
    protected override void Awake()
    {
        base.Awake();
        AddCallbacks();
    }

    private void AddCallbacks()
    {
        UIView.OnShowUI += OnShowUI;
        UIView.OnHideUI += OnHideUI;
    }

    /// <summary>
    ///  UI 객체를 반환하는 함수.
    ///  UI를 Show/Hide하는 기능은 포함하지 않음.
    ///  T는 UIView의 하위클래스, 동일 이름의 프리팹(T를 컴포넌트로 갖는)이 "Resources/UI"(Path)에 존재해야 함
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetUIComponent<T>() where T : UIView
    {
        string key = typeof(T).Name;
        if (!_uiDic.ContainsKey(key))
        {
            // 요청 시점에 해당 UI 컴포넌트가 없다면 Load
            GameObject prefab = Resources.Load<GameObject>($"UI/{key}");     //TODO : Set Path
            if (!prefab)
            {
#if UNITY_EDITOR
                Debug.LogError($"UI Prefab 로드 실패 : {key}");
#endif
                return null;
            }
            GameObject obj = Instantiate(prefab);
            if (!obj.TryGetComponent<T>(out T component))
            {
#if UNITY_EDITOR
                Debug.LogError($"Get UI Component 실패 : {key}");
#endif
                return null;
            }
            _uiDic.Add(key, obj.GetComponent<T>());
            _uiDic[key].gameObject.SetActive(false);
        }
        
        return  _uiDic[key] as T;
    }

    public bool TryGetUIComponent<T>(out T uiComponent) where T : UIView
    {
        string key = typeof(T).Name;
        if (!_uiDic.ContainsKey(key))
        {
            // 요청 시점에 해당 UI 컴포넌트가 없다면 Load
            GameObject prefab = Resources.Load<GameObject>($"UI/{key}"); //TODO : Set Path;
            if (!prefab)
            {
#if UNITY_EDITOR
                Debug.LogError($"UI Prefab 로드 실패 : {key}");
#endif
                uiComponent = null;
                return false;
            }
            GameObject obj = Instantiate(prefab);
            if (!obj.TryGetComponent<T>(out T component))
            {
#if UNITY_EDITOR
                Debug.LogError($"Get UI Component 실패 : {key}");
#endif
                uiComponent = null;
                return false;
            }
            _uiDic.Add(key, component);
            _uiDic[key].gameObject.SetActive(false);
        }
        
        uiComponent = _uiDic[key] as T;
        return true;
    }

    // UIView 하위 타입에 따른 관리
    private void OnShowUI(UIView view)
    {
        if (view is UIViewStackable stackable)
        {
            _uiStack.Push(stackable);
        }

        _uiActivated.Add(view);
    }

    // UIView 하위 타입에 따른 관리
    private void OnHideUI(UIView view)
    {
        if (view is UIViewStackable stackable)
        {
#if UNITY_EDITOR
            Debug.Assert(_uiStack.Peek() == stackable);
#endif
            _uiStack.Pop();
        }

        _uiActivated.Remove(view);
    }

    /// <summary>
    /// 활성화되어 있는 모든 UIViewStackable들을 닫는(Hide) 기능.
    /// </summary>
    public void HideStack()
    {
        foreach (UIView view in _uiStack)
        {
            view.Hide();
        }
    }

    /// <summary>
    /// 활성화되어 있는 모든 UIView를 닫는(Hide)기능
    /// </summary>
    public void HideAll()
    {
        foreach (UIView view in _uiActivated)
        {
            view.Hide();
        }
    }

    public bool IsViewActivated(UIView view)
    {
        return _uiActivated.Contains(view);
    }

    public bool IsUIStackEmpty()
    {
        return _uiStack.Count == 0;
    }

    /// <summary>
    /// 씬 전환 등의 UIView 객체 파괴 시점에 collection들을 초기화
    /// </summary>
    public void ClearCollections()
    {
        _uiDic.Clear();
        _uiActivated.Clear();
        _uiStack.Clear();
    }
}