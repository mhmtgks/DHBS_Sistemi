using DHBS_Sistemi.Controllers;

namespace DHBS_Sistemi.Models
{
    public class Router
    {

       ValidationController_HK_ HK_ = new ValidationController_HK_();
        ValidationController_User_ User_= new ValidationController_User_();
        ValidationController_Doktor_ Doktor_ = new ValidationController_Doktor_();
        public string Url {get; set; } 
     /*   public Router(ValidationController_Doktor_ _Doktor_,ValidationController_HK_ _HK_,ValidationController_User_ _User_)
        {
            HK_ = _HK_;
            User_ = _User_; 
            Doktor_ = _Doktor_; 
        }*/        public string FirstRouting(string tkn)
        {
            if (HK_.Validate(tkn))
            {

                return Url="AnaSayfa";

            }
            else if (User_.Validate(tkn))
            {
                return Url ="AnaSayfaH";

            }else if (Doktor_.Validate(tkn))
            {
                return Url = "AnasayfaD";
            }
            return Url;
        }


    }
}
