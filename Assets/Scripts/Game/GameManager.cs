using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const int BoardSize = 8;
    Chessboard board;
    private Player whitePlayer;
    private Player blackPlayer;
    public Player activePlayer;
    public bool gameOver {get; set;}
    void Start()
    {
        board = FindObjectOfType<Chessboard>();
        gameOver = false;
        InitializePlayers();
        StartNewGame();
    }

    private void InitializePlayers(){
        Player white = new Player("White",board);
        Player black = new Player("Black",board);
        whitePlayer = white;
        blackPlayer = black;
        foreach(Piece piece in board.AllPieces()){
            if(piece.team == "White"){
                piece.player = white;
                whitePlayer.AddActivePiece(piece);
            }else{
                piece.player = black;
                blackPlayer.AddActivePiece(piece);
            } 
        }
    }

    void StartNewGame(){
        board.SetDependencies(this);
        activePlayer = whitePlayer;
        GenerateAllMovesOfPlayer(activePlayer);
    }

    public void EndTurn(){
        if(gameOver){
            GameOverSequence();
        }
        GenerateAllMovesOfPlayer(activePlayer);
        GenerateAllMovesOfPlayer(getOtherPlayer(activePlayer));
        ChangeTeam();
    }
 
    private void GenerateAllMovesOfPlayer(Player player){
        player.GenerateAllPossibleMoves();
    }

    private Player getOtherPlayer(Player player){
        return player == whitePlayer ?  blackPlayer : whitePlayer; 
    }

    private void ChangeTeam(){
        activePlayer = activePlayer == whitePlayer ? blackPlayer : whitePlayer;
    }

    public bool IsTeamTurn(string team){
        return activePlayer.PlayerColor == team;
    }

    private void GameOverSequence()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RemovePieceFromPlayer(Piece piece){
        activePlayer.RemoveActivePiece(piece);
    }

    public void RemovePieceFromOtherPlayer(Piece piece){
        getOtherPlayer(activePlayer).RemoveActivePiece(piece);
    }

    public void AddPieceToPlayer(Piece piece){
        activePlayer.AddActivePiece(piece);
    }

    public void AddPieceToOtherPlayer(Piece piece){
        getOtherPlayer(activePlayer).AddActivePiece(piece);
    }

}
