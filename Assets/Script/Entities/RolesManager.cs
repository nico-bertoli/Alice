using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RolesManager
{
    public enum eRoles { PLAYER, PAWN, TOWER, BISHOP, HORSE}

    public static bool IsRoleLowerThan(eRoles _r1,eRoles _r2) {
        if (_r1<_r2) return true;
        return false;
    }
}
