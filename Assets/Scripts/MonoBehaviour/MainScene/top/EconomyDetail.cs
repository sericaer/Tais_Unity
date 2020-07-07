using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using ModelShark;

public class EconomyDetail : MonoBehaviour
{
    public Slider LocalCurrTaxSlider;
    public Text LocalCurrTaxText;

    public Slider ChaotingExpectTaxSlider;
    public Text ChaotingExpectTaxText;

    public Text surplusText;
    public Text consumeText;

    public Button btnConfirm;

    TaisEngine.Run.Economy economy;
    TaisEngine.Run.Chaoting chaoting;

    public void LocalCurrTaxSliderValueChanged(float value)
    {

        changedCurrTax = value - (float)economy.curr_tax_level;
    }

    public void OnConfirm()
    {
        if (changedCurrTax.Equals(0.0))
        {
            economy.currTaxChanged(changedCurrTax);
        }
        
        this.gameObject.SetActive(false);
    }

    public void OnCancel()
    {
        this.gameObject.SetActive(false);
    }

    private float changedCurrTax;

    private void OnEnable()
    {
        Timer.Pause();

        RefreshData();
    }

    private void OnDisable()
    {
        changedCurrTax = 0;
        Timer.unPause();
    }

    private void Awake()
    {
        
        economy = TaisEngine.Run.RunData.inst.economy;
        chaoting = TaisEngine.Run.RunData.inst.chaoting;

        LocalCurrTaxSlider.minValue = 0;
        LocalCurrTaxSlider.maxValue = (float)TaisEngine.Run.Economy.TAX_LEVEL.levelmax;

        ChaotingExpectTaxSlider.minValue = 0;
        ChaotingExpectTaxSlider.maxValue = (float)TaisEngine.Run.Economy.TAX_LEVEL.levelmax;

        changedCurrTax = 0;

        ChaotingExpectTaxSlider.interactable = false;
    }

    private void RefreshData()
    {
        ChaotingExpectTaxSlider.value = (float)chaoting.tax_level;
        LocalCurrTaxSlider.value = (float)economy.curr_tax_level;

        ChaotingExpectTaxText.text = "-" + chaoting.expect_tax.ToString();
        LocalCurrTaxText.text = economy.currTax.ToString();

        LocalCurrTaxSlider.interactable = economy.local_tax_change_valid;
        //if(!LocalCurrTaxSlider.interactable)
        //{
        //    LocalCurrTaxSlider.GetComponent<TooltipTrigger>().funcGetTooltipStr = () =>
        //    {
        //        return ("", TaisEngine.Mod.GetLocalString("VALID_DAYS_SPAN", economy.taxChangedDaysSpan, TaisEngine.GMDate.ToString(economy.validTaxChangedDays)));
        //    };
        //}
        //else
        //{
        //    LocalCurrTaxSlider.GetComponent<TooltipTrigger>().funcGetTooltipStr = () =>
        //    {
        //        return ("", "");
        //    };
        //}
    }

    private void Update()
    {
        LocalCurrTaxText.text = economy.getExpectTaxValue(LocalCurrTaxSlider.value).ToString();

        var surplus = double.Parse(LocalCurrTaxText.text) - double.Parse(ChaotingExpectTaxText.text);
        surplusText.text = surplus.ToString();
        //consumeText.text = TaisEngine.Defines.getExpectConsume(LocalCurrTaxSlider.value).ToString();
    }

}
