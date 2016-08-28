using UnityEngine;
using System.Collections;

public class HeroBlink : MonoBehaviour {

    public Material _mat = null;
    
    public bool SetBlinkColorAndOpacity(Color c, float opacity)
    {
        if(_mat!= null)
        {
            _mat.SetColor("_BlinkColor", c);
            _mat.SetFloat("_BlinkOpacity", opacity);
            return true;
        }
        return false;
    }
}
