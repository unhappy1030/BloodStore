using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BloodtypeGameAI : MonoBehaviour
{
    public bool isCalculationFinished;
    int[,] totalScore;

    public int[] FindOptimalIndex(BloodTypeGameSet gameSet){
        ResetTotalScore();
        Test(gameSet);
        return FindMaxScore();
    }

    void Test(BloodTypeGameSet gameSet){
        List<int[]> allOfNullIdxs = new List<int[]>();
        for(int i=0; i<3; i++){
            for(int j=0; j<3; j++){
                int[] idxs = new int[2]{i,j};
                if(!gameSet.IsAnyDeckFilled(idxs))
                    allOfNullIdxs.Add(idxs);
            }
        }

        foreach(int[] idxs in allOfNullIdxs){
            totalScore[idxs[0], idxs[1]] = EvaluateScore(gameSet, idxs, 0);
        }
    }

    int EvaluateScore(BloodTypeGameSet gameSet, int[] idxs, int repeat){
        if(repeat == 4) 
            return 0;
        
        int totalScore = 0;
        BloodTypeGameSet newGameSet = new BloodTypeGameSet(gameSet);
        newGameSet.PutOnTheDeck(idxs);
        if(newGameSet.IsGameEnd()){
            totalScore += (newGameSet.isPlayerWin) ? -10000 : 10000;
            return totalScore;
        }

        if(newGameSet.isPlayerTurn){
            int playerCount = newGameSet.CountOfTwoDotsInPlayerDeck();
            if(playerCount != 0){
                totalScore -= 3 * playerCount;
            }
        }
        else{
            int computerCount = newGameSet.CountOfTwoDotsInComputerDeck();
            if(computerCount != 0){
                totalScore += 3 * computerCount;
            }
        }

        newGameSet.ChangeTurn();

        List<int[]> allOfNullIdxs = new List<int[]>();
        List<int> eachScore = new List<int>();
        for(int i=0; i<3; i++){
            for(int j=0; j<3; j++){
                int[] nullIdxs = new int[2]{i,j};
                if(!newGameSet.IsAnyDeckFilled(nullIdxs))
                    allOfNullIdxs.Add(nullIdxs);
            }
        }

        foreach(int[] targetIdxs in allOfNullIdxs){
            newGameSet.currentBloodtype = "A+";
            eachScore.Add(EvaluateScore(newGameSet, targetIdxs, repeat+1));
            
            newGameSet.currentBloodtype = "B+";
            eachScore.Add(EvaluateScore(newGameSet, targetIdxs, repeat+1));
            
            newGameSet.currentBloodtype = "O+";
            eachScore.Add(EvaluateScore(newGameSet, targetIdxs, repeat+1));
            
            newGameSet.currentBloodtype = "AB+";
            eachScore.Add(EvaluateScore(newGameSet, targetIdxs, repeat+1));
        }

        if(newGameSet.isPlayerTurn){
            return totalScore + eachScore.IndexOf(eachScore.Min());
        }
        else{
            return totalScore + eachScore.IndexOf(eachScore.Max());
        }
    }

    int EvaluateForBloodTypeAndPosition(string bloodtype, int[] idxs){
        switch(bloodtype){
            case "O+" :
            case "A+" :
            {
                if(IsSameIdxs(idxs, 1) && IsSameIdxs(idxs, 3) && IsSameIdxs(idxs, 5) && IsSameIdxs(idxs, 7))
                    return 3;
                else if(IsSameIdxs(idxs, 0) && IsSameIdxs(idxs, 2) && IsSameIdxs(idxs, 6) && IsSameIdxs(idxs, 8))
                    return 1;
                else
                    return 0;
            }
            case "B+" :
            {
                if(IsSameIdxs(idxs, 1) && IsSameIdxs(idxs, 3) && IsSameIdxs(idxs, 5) && IsSameIdxs(idxs, 7))
                    return 1;
                else if(IsSameIdxs(idxs, 0) && IsSameIdxs(idxs, 2) && IsSameIdxs(idxs, 6) && IsSameIdxs(idxs, 8))
                    return 2;
                else
                    return 0;
            }
            case "AB+" :
            {
                if(IsSameIdxs(idxs, 1) && IsSameIdxs(idxs, 3) && IsSameIdxs(idxs, 5) && IsSameIdxs(idxs, 7))
                    return 3;
                else if(IsSameIdxs(idxs, 0) && IsSameIdxs(idxs, 2) && IsSameIdxs(idxs, 6) && IsSameIdxs(idxs, 8))
                    return 0;
                else
                    return 5;
            }
            default : return 0;
        }

    }

    bool IsSameIdxs(int[] idxs, int compareIdx){
        return (idxs[0] == compareIdx/3 && idxs[1] == compareIdx%3);
    }

    void ResetTotalScore(){
        totalScore = new int[3,3];
    }

    int[] FindMaxScore(){
        int maxScore = totalScore[0,0];
        int[] maxIdx = new int[2]{0,0};
        for(int i=0; i<3; i++){
            for(int j=0; j<3; j++){
                if(maxScore < totalScore[i,j]){
                    maxScore = totalScore[i,j];
                    maxIdx[0] = i;
                    maxIdx[1] = j;
                }
            }
        }

        isCalculationFinished = true;
        return maxIdx;
    } 
}