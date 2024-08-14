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
        playerDeck = new string[3,3];
        computerDeck = new string[3,3];
        SetNextRandomBloodType();
        isPlayerWin = false;
    }
    public BloodTypeGameSet(BloodTypeGameSet targetSet){
        playerDeck = new string[3,3];
        for(int i=0; i<3; i++){
            for(int j=0; j<3; j++){
                playerDeck[i,j] = targetSet.playerDeck[i,j];
            }
        }
        computerDeck = new string[3,3];
        for(int i=0; i<3; i++){
            for(int j=0; j<3; j++){
                computerDeck[i,j] = targetSet.computerDeck[i,j];
            }
        }
        currentBloodtype = targetSet.currentBloodtype;
        isPlayerTurn = targetSet.isPlayerTurn;
        isPlayerWin = targetSet.isPlayerWin;
    }

    public void PutOnTheDeck(int[] idxs){
        if(isPlayerTurn)
            PutOnPlayerDeck(idxs, currentBloodtype);
        else
            PutOnComputerDeck(idxs, currentBloodtype);
    }

    void PutOnPlayerDeck(int[] idxs, string bloodtype){
        playerDeck[idxs[0], idxs[1]] = bloodtype;
        RemoveBloodClot(playerDeck, idxs, bloodtype);
        RemoveBloodClot(computerDeck, idxs, bloodtype);
    }

    void PutOnComputerDeck(int[] idxs, string bloodtype){
        computerDeck[idxs[0], idxs[1]] = bloodtype;
        RemoveBloodClot(playerDeck, idxs, bloodtype);
        RemoveBloodClot(computerDeck, idxs, bloodtype);
    }

    void RemoveBloodClot(string[,] targetDeck, int[] idxs, string newBloodType){
        if(idxs[0] > 0 && targetDeck[idxs[0]-1, idxs[1]] != null) // up
        {
            if(!IsAbleToGiveBlood(newBloodType, targetDeck[idxs[0]-1, idxs[1]])){
                targetDeck[idxs[0]-1, idxs[1]] = null;
            }
        }

        if(idxs[0] < 2 && targetDeck[idxs[0]+1, idxs[1]] != null) // down
        {
            if(!IsAbleToGiveBlood(newBloodType, targetDeck[idxs[0]+1, idxs[1]])){
                targetDeck[idxs[0]+1, idxs[1]] = null;
            }
        }
        
        if(idxs[1] < 2 && targetDeck[idxs[0], idxs[1]+1] != null) // right
        {
            if(!IsAbleToGiveBlood(newBloodType, targetDeck[idxs[0],idxs[1]+1])){
                targetDeck[idxs[0],idxs[1]+1] = null;
            }
        }

        if(idxs[1] > 0 && targetDeck[idxs[0], idxs[1]-1] != null) // left
        {
            if(!IsAbleToGiveBlood(newBloodType, targetDeck[idxs[0], idxs[1]-1])){
                targetDeck[idxs[0], idxs[1]-1] = null;
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

    public bool IsAnyDeckFilled(int[] idxs){
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
        if(IsAllFilled(targetDeck, 0, 4, 8) || IsAllFilled(targetDeck, 1, 4, 7) || IsAllFilled(targetDeck, 2, 4, 6) || IsAllFilled(targetDeck, 3, 4, 5)){
            return true;
        }

        if(IsAllFilled(targetDeck, 0, 3, 6) || IsAllFilled(targetDeck, 0, 1, 2)){
            return true;
        }

        if(IsAllFilled(targetDeck, 6, 7, 8) || IsAllFilled(targetDeck, 2, 5, 8)){
            return true;
        }

        return false;
    }

    bool IsAllFilled(string[,] targetDeck, int i1, int i2, int i3){
        return (targetDeck[i2/3, i2%3] != null && targetDeck[i1/3, i1%3] != null && targetDeck[i3/3, i3%3] != null);
    }

    public int CountOfTwoDotsInPlayerDeck(){
        int count = 0;
        List<int> nullIdxsForRow = new List<int>();
        List<int> nullIdxsForColon = new List<int>();

        for(int i=0; i<3; i++){ // 행 & 열
            nullIdxsForRow.Clear();
            nullIdxsForColon.Clear();
            for(int j=0; j<3; j++){
                if(playerDeck[i,j] == null)
                    nullIdxsForRow.Add(i);
                if(playerDeck[j,i] == null)
                    nullIdxsForColon.Add(j);
            }
            if(nullIdxsForRow.Count == 1 && computerDeck[i,nullIdxsForRow[0]] == null)
                count++;
            if(nullIdxsForColon.Count == 1 && computerDeck[nullIdxsForColon[0],i] == null)
                count++;
        }

        if(computerDeck[2,2] == null && playerDeck[0,0] != null && playerDeck[1,1] != null 
            || computerDeck[1,1] == null && playerDeck[0,0] != null && playerDeck[2,2] != null
            || computerDeck[0,0] == null && playerDeck[1,1] != null && playerDeck[2,2] != null)
        {
            count++;
        }

        if(computerDeck[0,2] == null && playerDeck[2,0] != null && playerDeck[1,1] != null 
            || computerDeck[1,1] == null && playerDeck[2,0] != null && playerDeck[0,2] != null
            || computerDeck[2,0] == null && playerDeck[1,1] != null && playerDeck[0,2] != null)
        {
            count++;
        }

        return count;
    }

    public int CountOfTwoDotsInComputerDeck(){
        int count = 0;
        List<int> nullIdxsForRow = new List<int>();
        List<int> nullIdxsForColon = new List<int>();

        for(int i=0; i<3; i++){ // 행 & 열
            nullIdxsForRow.Clear();
            nullIdxsForColon.Clear();
            for(int j=0; j<3; j++){
                if(computerDeck[i,j] == null)
                    nullIdxsForRow.Add(i);
                if(computerDeck[j,i] == null)
                    nullIdxsForColon.Add(j);
            }
            if(nullIdxsForRow.Count == 1 && playerDeck[i,nullIdxsForRow[0]] == null)
                count++;
            if(nullIdxsForColon.Count == 1 && playerDeck[nullIdxsForColon[0],i] == null)
                count++;
        }

        if(playerDeck[2,2] == null && computerDeck[0,0] != null && computerDeck[1,1] != null 
            || playerDeck[1,1] == null && computerDeck[0,0] != null && computerDeck[2,2] != null
            || playerDeck[0,0] == null && computerDeck[1,1] != null && computerDeck[2,2] != null)
        {
            count++;
        }

        if(playerDeck[0,2] == null && computerDeck[2,0] != null && computerDeck[1,1] != null 
            || playerDeck[1,1] == null && computerDeck[2,0] != null && computerDeck[0,2] != null
            || playerDeck[2,0] == null && computerDeck[1,1] != null && computerDeck[0,2] != null)
        {
            count++;
        }

        return count;
    }

    public void ChangeTurn(){
        isPlayerTurn = !isPlayerTurn;
    }
}
