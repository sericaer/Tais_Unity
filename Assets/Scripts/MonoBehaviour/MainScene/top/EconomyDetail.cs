using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TaisEngine.ModManager;
using TaisEngine.Run;

//using ModelShark;

public class EconomyDetail : MonoBehaviour
{
    public static string prefabsName = "Prefabs/EconomyDetail";

    public ExpendDetail expendCountryTax;
    public InComeDetail incomePopTax;

    public GameObject prefabMessageBox;

    public Text surplus;
    //public Slider LocalCurrTaxSlider;
    //public Text LocalCurrTaxText;

    //public Slider ChaotingExpectTaxSlider;
    //public Text ChaotingExpectTaxText;

    //public Text surplusText;
    //public Text consumeText;

    public Button btnConfirm;
    public Button btnCancel;

    public double surplusValue;

    public bool canCancel = true;

    //public GameObject prefabConfirmDialog;

    //TaisEngine.Run.Economy economy;
    //TaisEngine.Run.Chaoting chaoting;

    //public void LocalCurrTaxSliderValueChanged(float value)
    //{

    //    changedCurrTax = value - (float)economy.curr_tax_level;
    //}

    public void OnConfirm()
    {
        //var confirmDialog = Instantiate(prefabConfirmDialog, this.transform) as GameObject;

        //var dialog = confirmDialog.GetComponent<DialogNextChangedConfirm>();

        //dialog.desc = LocalString.Get("NEXT_CHANGED_DESC", 
        //                              CommonDef.TaxLevel.Get().tax_change_intervl.Value, 
        //                              GMDate.Parse(RunData.inst.date.total_days + CommonDef.TaxLevel.Get().tax_change_intervl.Value).ToString());
        //dialog.act = () =>
        //{
        //    if (!changedCurrTax.Equals(0.0f))
        //    {
        //        economy.currTaxChanged(changedCurrTax);
        //    }

        //    this.gameObject.SetActive(false);
        //}; 
        if(surplusValue + RunData.inst.economy.value < 0)
        {
            var gameObject = Instantiate(Resources.Load(MessageBox.prefabsName), this.transform) as GameObject;
            gameObject.GetComponent<MessageBox>().desc.text = LocalString.Get("ECONOMY_NOT_SUPPORT_CURR_DEFICILT");
            return;
        }

        expendCountryTax.Confirm();
        incomePopTax.Confirm();

        Destroy(this.gameObject);
    }


    public void OnCancel()
    {
        Destroy(this.gameObject);
    }

    //private float changedCurrTax;

    private void OnEnable()
    {
        //Timer.Pause();

        //RefreshData();

        btnCancel.interactable = canCancel;
    }

    private void OnDisable()
    {
        canCancel = true;
    }

    private void Awake()
    {
        expendCountryTax.gmExpend = RunData.inst.economy.countryTax;
        incomePopTax.gmIncome = RunData.inst.economy.popTax;

        //economy = TaisEngine.Run.RunData.inst.economy;
        //chaoting = TaisEngine.Run.RunData.inst.chaoting;

        //LocalCurrTaxSlider.minValue = 0;
        //LocalCurrTaxSlider.maxValue = (float)TaisEngine.Run.Economy.TAX_LEVEL.levelmax;

        //ChaotingExpectTaxSlider.minValue = 0;
        //ChaotingExpectTaxSlider.maxValue = (float)TaisEngine.Run.Economy.TAX_LEVEL.levelmax;

        //changedCurrTax = 0;

        //ChaotingExpectTaxSlider.interactable = false;
    }



    //private void RefreshData()
    //{
    //    ChaotingExpectTaxSlider.value = (float)chaoting.tax_level;
    //    LocalCurrTaxSlider.value = (float)economy.curr_tax_level;

    //    ChaotingExpectTaxText.text =  chaoting.expect_tax.ToString();
    //    LocalCurrTaxText.text = economy.currTax.ToString();

    //    LocalCurrTaxSlider.interactable = economy.local_tax_change_valid;
    //    //if(!LocalCurrTaxSlider.interactable)
    //    //{
    //    //    LocalCurrTaxSlider.GetComponent<TooltipTrigger>().funcGetTooltipStr = () =>
    //    //    {
    //    //        return ("", TaisEngine.Mod.GetLocalString("VALID_DAYS_SPAN", economy.taxChangedDaysSpan, TaisEngine.GMDate.ToString(economy.validTaxChangedDays)));
    //    //    };
    //    //}
    //    //else
    //    //{
    //    //    LocalCurrTaxSlider.GetComponent<TooltipTrigger>().funcGetTooltipStr = () =>
    //    //    {
    //    //        return ("", "");
    //    //    };
    //    //}
    //}

    private void Update()
    {
        //LocalCurrTaxText.text = economy.getExpectTaxValue(LocalCurrTaxSlider.value).ToString();

        //var surplus = double.Parse(LocalCurrTaxText.text) - double.Parse(ChaotingExpectTaxText.text);
        //surplusText.text = surplus.ToString();

        //consumeText.text = CommonDef.TaxLevel.getConsume(LocalCurrTaxSlider.value).ToString();
        surplusValue = incomePopTax.Num - expendCountryTax.Num;
        surplus.text = (surplusValue).ToString();
        //btnConfirm.interactable = expendCountryTax.isChanged || incomePopTax.isChanged;
    }

}
