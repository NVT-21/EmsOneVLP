using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Library
{
    public class FormatterGeneral : FormatterBase
    {
        private string m_Message = string.Empty;

        public FormatterGeneral(string Message)
        {
            m_Message = Message;
        }

        public override string Message
        {
            get
            {
                return m_Message;
            }
        }
    }
}
