// user libraries
#include "uart.h"
#include "config.h"

// platform libraries
#include <avr/io.h>
#include <util/setbaud.h>

void uart_init(void)
{
	UBRR0H = UBRRH_VALUE;
	UBRR0L = UBRRL_VALUE;
	
#if USE_2X
	UCSR0A |= (1 << U2X0);
#else
	UCSR0A &= ~(1 << U2X0);
#endif

	UCSR0B = 1 << TXEN0 | 1 << RXEN0;	//enable duplex
	UCSR0C = 1 << UCSZ00 | 1 << UCSZ01; //8-N-1
}

uint8_t uart_available(void)
{
	return !!(UCSR0A & (1 << RXC0));
}

void uart_putchar(uint8_t c)
{
	while ((UCSR0A & (1 << UDRE0)) == 0) 
		;
		
	UDR0 = c;
}

uint8_t uart_getchar(void)
{
	while (!uart_available()) 
		;
	
	return UDR0;	
}
