#include <stdio.h>
#include <stdlib.h>
#include <SDL2/SDL.h>
#include <SDL2/SDL_image.h>
#include <SDL2/SDL_ttf.h>
#include <SDL2/SDL_mixer.h>

int main(int argc, char* argv[]) {
    SDL_Window* window = NULL;
    SDL_Renderer* renderer = NULL;
    SDL_Texture* icon = NULL;
    SDL_Texture* background = NULL;
    SDL_Rect square;
    Mix_Music* music = NULL;
    TTF_Font* font = NULL;
    SDL_Color textColor = {255, 255, 255, 255};
    SDL_Surface* textSurface = NULL;
    SDL_Texture* textTexture = NULL;
    SDL_Rect textRect;
    double ver = 0.01;

    if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_AUDIO) < 0) {
        printf("SDL could not initialize! SDL_Error: %s\n", SDL_GetError());
        return -1;
    }

    if (TTF_Init() == -1) {
        printf("TTF could not initialize! TTF_Error: %s\n", TTF_GetError());
        return -1;
    }

    window = SDL_CreateWindow("About Replycator", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 360, 500, SDL_WINDOW_SHOWN);
    if (!window) {
        printf("Window could not be created! SDL_Error: %s\n", SDL_GetError());
        return -1;
    }

    renderer = SDL_CreateRenderer(window, -1, SDL_RENDERER_ACCELERATED);
    if (!renderer) {
        printf("Renderer could not be created! SDL_Error: %s\n", SDL_GetError());
        return -1;
    }

    icon = IMG_LoadTexture(renderer, "src/icon.ico");
    SDL_SetWindowIcon(window, icon);

    background = IMG_LoadTexture(renderer, "src/background_about.gif");

    square.x = 40;
    square.y = 480;
    square.w = 280;
    square.h = 150;

    if (Mix_OpenAudio(44100, MIX_DEFAULT_FORMAT, 2, 2048) < 0) {
        printf("SDL_mixer could not initialize! SDL_mixer Error: %s\n", Mix_GetError());
        return -1;
    }

    music = Mix_LoadMUS("src/replycator.ogg");
    Mix_PlayMusic(music, -1);

    font = TTF_OpenFont("Ubuntu.ttf", 15);
    if (!font) {
        printf("Failed to load font! SDL_ttf Error: %s\n", TTF_GetError());
        return -1;
    }

    char replycatorText[50];
    sprintf(replycatorText, "Replycator %.2f", ver);
    textSurface = TTF_RenderText_Solid(font, replycatorText, textColor);
    textTexture = SDL_CreateTextureFromSurface(renderer, textSurface);
    textRect.x = 0;
    textRect.y = 108;
    textRect.w = textSurface->w;
    textRect.h = textSurface->h;

    SDL_Event e;
    int quit = 0;

    while (!quit) {
        while (SDL_PollEvent(&e) != 0) {
            if (e.type == SDL_QUIT) {
                quit = 1;
            }
        }

        SDL_RenderClear(renderer);
        SDL_RenderCopy(renderer, background, NULL, NULL);
        SDL_SetRenderDrawColor(renderer, 10, 10, 10, 255);
        SDL_RenderFillRect(renderer, &square);
        SDL_RenderCopy(renderer, textTexture, NULL, &textRect);
        SDL_RenderPresent(renderer);
    }

    SDL_DestroyTexture(background);
    SDL_DestroyTexture(icon);
    SDL_DestroyTexture(textTexture);
    SDL_FreeSurface(textSurface);
    TTF_CloseFont(font);
    Mix_FreeMusic(music);
    SDL_DestroyRenderer(renderer);
    SDL_DestroyWindow(window);
    TTF_Quit();
    Mix_CloseAudio();
    SDL_Quit();

    return 0;
}


