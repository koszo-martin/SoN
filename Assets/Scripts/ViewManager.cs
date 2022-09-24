using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance {get; private set;}
    [SerializeField]
    private View[] views;
    [SerializeField]
    private View defaultView;
    [SerializeField]
    private View currentView;
    [SerializeField]
    private bool autoInit;

    public void Awake(){
        Instance = this;
    }

    public void Start(){
        if(autoInit) Initialize();
    }

    public void Initialize(){
        foreach (View view in views){
            view.Initialize();

            view.Hide();
        }

        if(defaultView!=null) Show(defaultView);
    }

    public void Show<TView>(object args = null) where TView : View{
        foreach ( View view in views){
            if (view is TView){
                if (currentView != null) currentView.Hide();
                view.Show(args);
                currentView = view;
                break;
            }
        }
    }

    public void Show (View view, object args = null ){
        if (currentView != null) currentView.Hide();
        view.Show(args);
        currentView = view;
    }
}
