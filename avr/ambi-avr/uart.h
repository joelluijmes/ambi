#pragma once

// std libraries
#include <inttypes.h>

void uart_init(void);
uint8_t uart_available(void);
void uart_putchar(uint8_t c);
uint8_t uart_getchar(void);
