using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace M14_Task
{
    public class TemplateSelectorClientList : DataTemplateSelector
    {
        public DataTemplate PersonTemplate { get; set; }
        public DataTemplate OrganisationTemplate { get; set; }
        public DataTemplate NullTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item!= null) 
            {
                string type = item.GetType().ToString();
                switch (type)
                {
                    case "М13_Task1.Organization":
                        return OrganisationTemplate;
                    case "М13_Task1.Person":
                        return PersonTemplate;
                    default: return NullTemplate;
                }    
            }
            return NullTemplate;
        }
    }


    public class TemplateSelectorConsultantChoice : DataTemplateSelector
    {
 
        public DataTemplate NullTemplate { get; set; }

        public DataTemplate PersonConsultantTemplate { get; set; }

        public DataTemplate OrganisationConsultantTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                string type = item.GetType().ToString();
                switch (type)
                {
                    case "М13_Task1.Organization":
                        return OrganisationConsultantTemplate;
                    case "М13_Task1.Person":
                        return PersonConsultantTemplate;
                    default: return NullTemplate;
                }
            }
            return NullTemplate;
        }
    }


    public class TemplateSelectorClientBuilding : DataTemplateSelector
    {

        public DataTemplate NullTemplate { get; set; }

        public DataTemplate ChangePersonTemplate { get; set; }

        public DataTemplate ChangeOrganisationTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                string type = item.GetType().ToString();
                switch (type)
                {
                    case "М13_Task1.Organization":
                        return ChangeOrganisationTemplate;
                    case "М13_Task1.Person":
                        return ChangePersonTemplate;
                    default: return NullTemplate;
                }
            }
            return NullTemplate;
        }
    }



    public class TempAccountSelector : DataTemplateSelector
    {
        public DataTemplate AccountTemplate { get; set; }

        public DataTemplate DepositAccountTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                string type = item.GetType().ToString();
                switch (type)
                {
                    case "М13_Task1.Account":
                        return AccountTemplate;
                    case "М13_Task1.DepositAccount":
                        return DepositAccountTemplate;
                    default: return null;
                }
            }
            return null;
        }
    }

}

