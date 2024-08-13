using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodtypeGameAI : MonoBehaviour
{
    int[,] totalScore;

    void PlayTurn(BloodTypeGameSet gameSet){
        ResetTotalScore();
        FindMaxScore();
    }

    void Test(BloodTypeGameSet gameSet){
        List<int[]> allOfNullIdxs = new List<int[]>();
        for(int i=0; i<3; i++){
            for(int j=0; j<3; j++){
                int[] idxs = new int[2]{i,j};
                if(gameSet.IsAnyDeckFilled(idxs))
                    allOfNullIdxs.Add(idxs);
            }
        }

        foreach(int[] idxs in allOfNullIdxs){
            EvaluateScore(gameSet, idxs);
        }
    }

    int EvaluateScore(BloodTypeGameSet gameSet, int[] idxs){
        int totalScore = 0;
        BloodTypeGameSet newGameSet = new BloodTypeGameSet(gameSet);
        newGameSet.PutOnTheDeck(idxs);
        if(newGameSet.IsGameEnd()){
            totalScore += (newGameSet.isPlayerTurn) ? -1000 : 1000;
            return totalScore;
        }

        if(newGameSet.isPlayerTurn){
            int playerCount = newGameSet.CountOfTwoDots(newGameSet.playerDeck);
            if(playerCount != 0){
                totalScore -= 3 * playerCount;
            }
        }
        else{
            int computerCount = newGameSet.CountOfTwoDots(newGameSet.computerDeck);
            if(computerCount != 0){
                totalScore += 3 * computerCount;
            }
        }

        for(int i=0; i<3; i++){
            for(int j=0; j<3; j++){
                int[] newIdxs = new int[2]{i,j};

            }
        }

        return totalScore;
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
        return maxIdx;
    } 
}