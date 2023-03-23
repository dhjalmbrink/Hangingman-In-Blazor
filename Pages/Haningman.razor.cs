using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using BlazorHaningman;
using BlazorHaningman.Shared;
using BlazorHaningman.Model;
using BlazorHaningman.Services;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;

namespace BlazorHaningman.Pages
{
    public partial class Haningman
    {
        bool startedGame { get; set; } = false;
        string newWord;
        string yourLetter;
        List<RandomWord> letterList = new List<RandomWord>();
        int lives = 8;
        int lettersLeft = 0;
        List<string> guessedLetter = new List<string>();
        Regex validCharacters = new Regex("^[a-zA-Z]$");
        string infoText = string.Empty;
        string startNewGame = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            startNewGame = "Start game";
        }

        async Task startGame()
        {
            newWord = string.Empty;
            newWord = await wordService.GetRandomWord();
            lives = 8;
            lettersLeft = 0;
            guessedLetter = new List<string>();
            infoText = string.Empty;
            letterList = new List<RandomWord>();
            letterCheck(string.Empty);
            startedGame = true;
            startNewGame = "Restart";
            yourLetter = string.Empty;
        }

        void onEnterClick(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                yourGuess();
            }
        }

        void yourGuess()
        {
            if (!string.IsNullOrWhiteSpace(yourLetter) && newWord != null)
            {
                infoText = string.Empty;
                var letter = yourLetter.ToUpper();
                if (!validCharacters.IsMatch(letter))
                {
                    infoText = "Looks like something is wrong with your letter, try again";
                }
                else
                {
                    if (!guessedLetter.Contains(letter))
                    {
                        letterCheck(letter);
                    }
                    else
                    {
                        infoText = "Looks like you have tried this one before, try again";
                    }
                }

                yourLetter = string.Empty;
            }
            else
            {
                infoText = "Missing letter";
                yourLetter = string.Empty;
            }
        }

        void letterCheck(string? tryLetter)
        {
            if (!letterList.Any() && string.IsNullOrEmpty(tryLetter) && newWord != null)
            {
                newWord = newWord.ToUpper();
                foreach (char letter in newWord)
                {
                    var letterInWord = new RandomWord{Letter = letter.ToString(), LetterShowing = ("_ ")};
                    letterList.Add(letterInWord);
                    lettersLeft++;
                }
            }
            else if (!string.IsNullOrEmpty(tryLetter) && letterList != null)
            {
                tryLetter.ToUpper();
                for (var i = 0; i < letterList.Count; i++)
                {
                    if (letterList[i].Letter == tryLetter)
                    {
                        letterList[i].LetterShowing = tryLetter;
                        lettersLeft--;
                    }
                }

                if (!newWord.Contains(tryLetter))
                {
                    lives--;
                    infoText = "Letter is not used";
                    tryLetter = tryLetter.ToLower();
                }

                guessedLetter.Add(tryLetter);
                if (lettersLeft == 0)
                {
                    infoText = "You Won, with " + lives + " lives left";
                    startNewGame = "Play New Game";
                }

                if (lives == 0)
                {
                    infoText = "You lost, the word was " + newWord;
                    startNewGame = "Play New Game";
                }
            }
        }
    }
}