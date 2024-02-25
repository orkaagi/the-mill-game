using System;
using MillGame.Model;

namespace MillGame.Persistence
{
    public interface IMillGameDataAccess
    {
        Task<(Int32, Actions, Int32, Board)> LoadAsync(String path);
        Task SaveAsync(String path, Board board, Int32 currentPlayer, Actions lastAction, Int32 lasClicked);
    }
}