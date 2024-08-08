using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BloodTypeGameSet
{
    public string[,] playerDeck;
    public string[,] computerDeck;
    public string currentBloodtype;

    public void ResetDeckList(){
        playerDeck = new string[3,3];
        computerDeck = new string[3,3];
    }

    public void CopyDeckList(string[,] newPlayerDeck, string[,] newComputerDeck){
        playerDeck = new string[3,3];
        for(int i=0; i<3; i++){
            for(int j=0; j<3; j++){
                playerDeck[i,j] = newPlayerDeck[i,j];
            }
        }

        computerDeck = new string[3,3];
        for(int i=0; i<3; i++){
            for(int j=0; j<3; j++){
                computerDeck[i,j] = newComputerDeck[i,j];
            }
        }
    }

    public void PutOnPlayerDeck(int[] idxs, string bloodtype){
        playerDeck[idxs[0], idxs[1]] = bloodtype;
        CheckDeckStatus(playerDeck, idxs, bloodtype);
        CheckDeckStatus(computerDeck, idxs, bloodtype);
    }

    public void PutOnComputerDeck(int[] idxs, string bloodtype){
        computerDeck[idxs[0], idxs[1]] = bloodtype;
        CheckDeckStatus(playerDeck, idxs, bloodtype);
        CheckDeckStatus(computerDeck, idxs, bloodtype);
    }

    void CheckDeckStatus(string[,] targetDeck, int[] idxs, string newBloodType){
        if(idxs[0] > 0 && playerDeck[idxs[0]-1, idxs[1]] != null) // up
        {
            if(!isAbleToGiveBlood(newBloodType, playerDeck[idxs[0]-1, idxs[1]])){
                playerDeck[idxs[0]-1, idxs[1]] = null;
            }
        }

        if(idxs[0] < 2 && playerDeck[idxs[0]+1, idxs[1]] != null) // down
        {
            if(!isAbleToGiveBlood(newBloodType, playerDeck[idxs[0]+1, idxs[1]])){
                playerDeck[idxs[0]+1, idxs[1]] = null;
            }
        }
        
        if(idxs[1] < 2 && playerDeck[idxs[0], idxs[1]+1] != null) // right
        {
            if(!isAbleToGiveBlood(newBloodType, playerDeck[idxs[0],idxs[1]+1])){
                playerDeck[idxs[0],idxs[1]+1] = null;
            }
        }

        if(idxs[1] > 0 && playerDeck[idxs[0], idxs[1]-1] != null) // left
        {
            if(!isAbleToGiveBlood(newBloodType, playerDeck[idxs[0], idxs[1]-1])){
                playerDeck[idxs[0], idxs[1]-1] = null;
            }
        }
    }

    bool isAbleToGiveBlood(string newBloodType, string currentBloodType){
        if(newBloodType == "A+"){
            return (currentBloodType == "A+" || currentBloodType == "AB+");
        }
        else if(newBloodType == "B+"){
            return (currentBloodType == "B+" || currentBloodType == "AB+");
        }
        else if(newBloodType == "AB+"){
            return (currentBloodType == "AB+");
        }
        else{
            return true;
        }
    }

    public string MakeRandomBloodType(){
        float[] bloodtypeRate = {0.38f, 0.1f, 0.03f, 0.49f}; // A B AB O
        float randomPoint = UnityEngine.Random.value;

        int index = -1;
        if(randomPoint == 1){
            index = bloodtypeRate.Length - 1;
        }
        else{
            float cumulativeProb = 0f;
            for(int i=0; i<bloodtypeRate.Length; i++){
                cumulativeProb += bloodtypeRate[i];
                if(randomPoint < cumulativeProb){
                    index = i;
                    break;
                }
            }
        }

        string bloodtype;
        if(index == 0){
            bloodtype = "A+";
        }
        else if(index == 1){
            bloodtype = "B+";
        }
        else if(index == 2){
            bloodtype = "AB+";
        }
        else if(index == 3){
            bloodtype = "O+";
        }
        else{
            bloodtype = "ERROR";
        }

        return bloodtype;
    }

    
    public void SetCurrentBloodType(string currentBloodtype){
        this.currentBloodtype = currentBloodtype;
    }
}
