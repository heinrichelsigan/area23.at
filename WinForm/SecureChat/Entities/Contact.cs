using Area23.At.Framework.Core.CqrXs.CqrMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.WinForm.SecureChat.Entities
{

    /// <summary>
    /// Represents a contact entry
    /// </summary>
    public class Contact : CqrContact
    {
        
        /// <summary>
        /// <see cref="object[]">RowParams</see> gets an object array of row parameters to show in <see cref="System.Windows.Forms.DataGridView"/>
        /// </summary>
        public object[] RowParams
        {
            get
            {
                List<object> oList =
                [
                    ContactId,
                    Name,
                    Email,
                    Mobile,
                    Address,
                    ContactImage.ToDrawingBitmap(),
                ];
                return oList.ToArray();
            }
        }

    }

}
