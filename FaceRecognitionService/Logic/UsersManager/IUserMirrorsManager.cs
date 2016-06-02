using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognitionService.Logic.UsersManager
{
    interface IUserMirrorsManager
    {
        int[] getUsersMirrors(int userID);
        void addUsersMirror(int userID, int mirrorID);
        void deleteUsersMirror(int userID, int mirrorID);
        void deleteUsersMirrors(int userID);
    }
}
