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
    public bool isPlayerTurn;
    public bool isPlayerWin;

    public BloodTypeGameSet(){
        ResetDeckList();
        SetNextRandomBloodType();
        isPlayerWin = false;
    }

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

    public void PutOnTheDeck(int[] idxs){
        if(isPlayerTurn)
            PutOnPlayerDeck(idxs, currentBloodtype);
        else
            PutOnComputerDeck(idxs, currentBloodtype);
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
        // Player
        if(idxs[0] > 0 && playerDeck[idxs[0]-1, idxs[1]] != null) // up
        {
            if(!IsAbleToGiveBlood(newBloodType, playerDeck[idxs[0]-1, idxs[1]])){
                playerDeck[idxs[0]-1, idxs[1]] = null;
            }
        }

        if(idxs[0] < 2 && playerDeck[idxs[0]+1, idxs[1]] != null) // down
        {
            if(!IsAbleToGiveBlood(newBloodType, playerDeck[idxs[0]+1, idxs[1]])){
                playerDeck[idxs[0]+1, idxs[1]] = null;
            }
        }
        
        if(idxs[1] < 2 && playerDeck[idxs[0], idxs[1]+1] != null) // right
        {
            if(!IsAbleToGiveBlood(newBloodType, playerDeck[idxs[0],idxs[1]+1])){
                playerDeck[idxs[0],idxs[1]+1] = null;
            }
        }

        if(idxs[1] > 0 && playerDeck[idxs[0], idxs[1]-1] != null) // left
        {
            if(!IsAbleToGiveBlood(newBloodType, playerDeck[idxs[0], idxs[1]-1])){
                playerDeck[idxs[0], idxs[1]-1] = null;
            }
        }

        // Computer
        if(idxs[0] > 0 && computerDeck[idxs[0]-1, idxs[1]] != null) // up
        {
            if(!IsAbleToGiveBlood(newBloodType, computerDeck[idxs[0]-1, idxs[1]])){
                computerDeck[idxs[0]-1, idxs[1]] = null;
            }
        }

        if(idxs[0] < 2 && computerDeck[idxs[0]+1, idxs[1]] != null) // down
        {
            if(!IsAbleToGiveBlood(newBloodType, computerDeck[idxs[0]+1, idxs[1]])){
                computerDeck[idxs[0]+1, idxs[1]] = null;
            }
        }
        
        if(idxs[1] < 2 && computerDeck[idxs[0], idxs[1]+1] != null) // right
        {
            if(!IsAbleToGiveBlood(newBloodType, computerDeck[idxs[0],idxs[1]+1])){
                computerDeck[idxs[0],idxs[1]+1] = null;
            }
        }

        if(idxs[1] > 0 && computerDeck[idxs[0], idxs[1]-1] != null) // left
        {
            if(!IsAbleToGiveBlood(newBloodType, computerDeck[idxs[0], idxs[1]-1])){
                computerDeck[idxs[0], idxs[1]-1] = null;
            }
        }
    }

    bool IsAbleToGiveBlood(string newBloodType, string currentBloodType){
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

    public void SetNextRandomBloodType(){
        currentBloodtype = MakeRandomBloodType();
    }

    string MakeRandomBloodType(){
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

    public bool IsFilled(int[] idxs){
        return (playerDeck[idxs[0], idxs[1]] != null || computerDeck[idxs[0], idxs[1]] != null);
    }

    public bool IsGameEnd(){
        int playerCount = 0;
        int computerCount = 0;
        for(int i=0; i<3; i++){
            for(int j=0; j<3; j++){
                if(playerDeck[i,j] != null){
                    playerCount++;
                }
                if(computerDeck[i,j] != null){
                    computerCount++;
                }
            }
        }

        if(playerCount+computerCount == 9){
            isPlayerWin = (playerCount > computerCount);
            return true;
        }

        if(IsMakeOneLine(playerDeck)){
            isPlayerWin = true;
            return true;
        }
        
        if(IsMakeOneLine(computerDeck)){
            isPlayerWin = false;
            return true;
        }

        return false;
    }

    bool IsMakeOneLine(string[,] targetDeck){
        if(IsMakeOneLine(targetDeck, 0, 4, 8) || IsMakeOneLine(targetDeck, 1, 4, 7) || IsMakeOneLine(targetDeck, 2, 4, 6) || IsMakeOneLine(targetDeck, 3, 4, 5)){
            return true;
        }

        if(IsMakeOneLine(targetDeck, 0, 3, 6) || IsMakeOneLine(targetDeck, 0, 1, 2)){
            return true;
        }

        if(IsMakeOneLine(targetDeck, 6, 7, 8) || IsMakeOneLine(targetDeck, 2, 5, 8)){
            return true;
        }

        return false;
    }

    bool IsMakeOneLine(string[,] targetDeck, int i1, int i2, int i3){
        return (targetDeck[i2/3, i2%3] != null && targetDeck[i1/3, i1%3] != null && targetDeck[i3/3, i3%3] != null);
    }

    public void ChangeTurn(){
        isPlayerTurn = !isPlayerTurn;
    }
}
