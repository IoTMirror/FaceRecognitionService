using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognitionService.Logic.UsersManager
{
    interface IUserMirrorsManager
    {
        List<int> getUsersMirrors(int userID);
        List<int> getMirrorsUsers(int mirrorID);
        void addUsersMirror(int userID, int mirrorID);
        void deleteUsersMirror(int userID, int mirrorID);
        void deleteUsersMirrors(int userID);
    }
}
