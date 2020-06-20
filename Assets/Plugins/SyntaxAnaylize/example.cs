using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SyntaxAnaylize;

public class example : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string script = @"
trigger = 
{
    AND = 
    {
        is.buffer_valid = {gm_pop.buffers, PINKUN_LEVEL2}
        is.true = gm_pop.is_consume
    }
}

occur_days =
{
    base = 4*30
    modifiy =
    {
        value = -2*60
        is.big_equal = {gm_pop.consume, 80}
    }
}

options = 
{
    OPTION_1 = 
    {
        selected = 
        {
            set.buffer_valid = {gm_pop.buffers, PINKUN_LEVEL1}
        }
    }
}
";
        MultiItem modItems = Syntax.Anaylize(script);

        var str = modItems.ToString();

        Debug.Log(str);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
