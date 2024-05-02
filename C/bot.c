
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <dirent.h>
#include <unistd.h>

#define MAX_FILE_NAME 256

void send_message(int mode) {
    if (mode == 1) {
        // replycator_api.send('social');
        printf("Sending messages in social mode\n");
    } else if (mode == 2) {
        // replycator_api.send('youtube-social');
        printf("Sending messages in youtube-social mode\n");
    }
}

void remove_files() {
    DIR *dir;
    struct dirent *entry;
    char file_path[MAX_FILE_NAME];

    dir = opendir("files/");
    if (dir == NULL) {
        printf("Error opening directory\n");
        return;
    }

    while ((entry = readdir(dir)) != NULL) {
        if (strcmp(entry->d_name, ".") != 0 && strcmp(entry->d_name, "..") != 0) {
            snprintf(file_path, MAX_FILE_NAME, "files/%s", entry->d_name);
            remove(file_path);
        }
    }

    closedir(dir);
    printf("Storage cleared\n");
}

int main() {
    printf("Replycator BOT 0.01\n");

    // Initialize mode
    int mode = 0;

    // Define commands
    char *commands[] = {
        "/social", "Режим отправки для соцсетей",
        "/youtube", "Режим отправки для Youtube",
        "/remove", "Очистка отправленных боту файлов из его хранилища",
        "/help", "Мини-инструкция по пользованию ботом",
        "/send", "Отправка сообщений в ранее выбраный режим",
        "/quit", "Завершение работы бота"
    };

    // Print hello message
    printf("Данный бот предназначен для добавлений сообщений, медиа, текста и отправки их в разные соцсети \nЧтобы добавить файлы для отправки в такие соцсети как телеграмм, твиттер, то нужно активировать команду /social \nЧтобы добавить видео для отправки в ютуб, то нужно активировать команду /youtube \nЧтобы очистить загруженные боту сообщения, активируйте команду /remove \nПо завершению добавлению файлов нужно активировать команду /send \nПриятного использования!\n");

    // Command loop
    char command[MAX_FILE_NAME];
    while (1) {
        printf("Enter command: ");
        fgets(command, MAX_FILE_NAME, stdin);
        command[strcspn(command, "\n")] = 0; // Remove newline character

        if (strcmp(command, "/social") == 0) {
            if (mode == 0 || mode == 2) {
                mode = 1;
                remove_files();
                printf("Social mode selected\n");
            } else {
                printf("Social mode is already selected\n");
            }
        } else if (strcmp(command, "/youtube") == 0) {
            if (mode == 0 || mode == 1) {
                mode = 2;
                remove_files();
                printf("Youtube mode selected\n");
            } else {
                printf("Youtube mode is already selected\n");
            }
        } else if (strcmp(command, "/remove") == 0) {
            remove_files();
        } else if (strcmp(command, "/help") == 0) {
            printf("Данный бот предназначен для добавлений сообщений, медиа, текста и отправки их в разные соцсети \nЧтобы добавить файлы для отправки в такие соцсети как телеграмм, твиттер, то нужно активировать команду /social \nЧтобы добавить видео для отправки в ютуб, то нужно активировать команду /youtube \nЧтобы очистить загруженные боту сообщения, активируйте команду /remove \nПо завершению добавлению файлов нужно активировать команду /send \nПриятного использования!\n");
        } else if (strcmp(command, "/send") == 0) {
            int file_count = 0;
            DIR *dir = opendir("files/");
            if (dir != NULL) {
                struct dirent *entry;
                while ((entry = readdir(dir)) != NULL) {
                    if (strcmp(entry->d_name, ".") != 0 && strcmp(entry->d_name, "..") != 0) {
                        file_count++;
                    }
                }
                closedir(dir);
            }

            if (file_count == 0) {
                printf("Nothing to send, folder is empty\n");
            } else {
                printf("Sending messages...\n");
                send_message(mode);
                printf("Messages sent\n");
            }
        } else if (strcmp(command, "/quit") == 0) {
            printf("Shutting down bot\n");
            break;
        } else {
            printf("Invalid command\n");
        }
    }

    return 0;
}