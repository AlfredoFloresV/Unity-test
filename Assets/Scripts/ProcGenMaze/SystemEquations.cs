using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemEquation 
{
    public static bool validPoint(ParametricEquation eq1, ParametricEquation eq2, Vector3 p)
    {
        float dist1 = Vector3.Distance(eq1.getP1(), p) + Vector3.Distance(p, eq1.getP2());
        float dist2 = Vector3.Distance(eq1.getP1(), eq1.getP2());

        float dist3 = Vector3.Distance(eq2.getP1(), p) + Vector3.Distance(p, eq2.getP2());
        float dist4 = Vector3.Distance(eq2.getP1(), eq2.getP2());

        if (Math.Abs(dist1 - dist2) < 0.01 && Math.Abs(dist3 - dist4) < 0.01)
            return true;

        return false;
    }

    public static bool hasSameBounds(ParametricEquation eq1, ParametricEquation eq2) 
    {
        if (Vector3.Distance(eq1.getP1(), eq2.getP1()) != 0 &&
           Vector3.Distance(eq1.getP1(), eq2.getP2()) != 0 &&
           Vector3.Distance(eq1.getP2(), eq2.getP1()) != 0 &&
           Vector3.Distance(eq1.getP2(), eq2.getP2()) != 0)
        {
            return false;
        }
        return true;
    }

    public static bool intersects(ParametricEquation eq1, ParametricEquation eq2) 
    {
        float delta = 0.001f;

        if (hasSameBounds(eq1, eq2))
        {
            //Debug.Log("same bounds");
            return false;
        }
        else 
        {
            //Equation for X dimension
            float epsilon = eq1.getP1().x - eq2.getP1().x;
            float alpha = eq1.getVector().x;
            float alpha2 = eq2.getVector().x + delta;

            //Equation for Z dimension
            float epsilon2 = eq2.getP1().z - eq1.getP1().z;
            float beta = eq1.getVector().z;
            float beta2 = eq2.getVector().z;

            //Debug.Log("epsilon " + epsilon + " alpha " + alpha + " alpha2 " + alpha2);
            //Debug.Log("epsilon2 " + epsilon2 + " beta " + beta + " beta2 " + beta2);

            //Sustitute X in Y for coef2=coef4
            float omega = (epsilon2 + ((beta2 * epsilon) / alpha2)) / (beta - ((beta2 * alpha) / alpha2));
            float omega2 = (epsilon + (alpha * omega)) / alpha2;

            //Debug.Log("omega " + omega);
            //Debug.Log("omega2 " + omega2);

            float z1 = eq1.getP1().y + eq1.getVector().y * omega;
            float z2 = eq2.getP1().y + eq2.getVector().y * omega2;

            //Debug.Log("z1 " + z1 + " z2 " + z2);

            if (Math.Abs(z1 - z2) < 0.01) //Intersection!!
            {
                float px = eq1.getP1().x + (alpha * omega);
                float py = eq1.getP1().y + (eq1.getVector().y * omega);
                float pz = eq1.getP1().z + (beta * omega);
                Vector3 intersection = new Vector3(px, py, pz);

                if (validPoint(eq1, eq2, intersection)) 
                {
                    return true;
                }

                //Debug.Log("Intersection outside of bounds");
                return false;
            }

            //Debug.Log("Not a intersection");
            return false;
        }
    }

}


public class ParametricEquation 
{
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 vector;

    public Vector3 getP1() 
    {
        return p1;
    }

    public Vector3 getP2() 
    {
        return p2;
    }

    public Vector3 getVector() 
    {
        return vector;
    }

    public ParametricEquation(Vector3 v1, Vector3 v2) 
    {
        p1 = v1;
        p2 = v2;
        vector = v2 - v1;
    }
}
