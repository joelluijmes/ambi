// user libraries
#include "config.h"
#include "neo_apa104.h"
#include "uart.h"

// platform libraries
#include <avr/io.h>
#include <avr/common.h>
#include <util/delay.h>

typedef struct color_t 
{
	uint8_t red;
	uint8_t green;
	uint8_t blue;
} color_t;

typedef struct frame_t
{
	uint16_t count;
	color_t pixels[FRAME_LED_MAX];
} frame_t;

static void read_frame(volatile frame_t* frame)
{
	frame->count = (uart_getchar() | (uart_getchar() << 8));
	for (uint16_t i = 0; i < frame->count; ++i)
	{
		frame->pixels[i].red = uart_getchar();
		frame->pixels[i].green = uart_getchar();
		frame->pixels[i].blue = uart_getchar();
	}
}

static void show_frame(const frame_t* frame)
{
	for (uint8_t i = 0; i < frame->count; ++i)
	{
		neo_write_rgb(
			frame->pixels[i].red,
			frame->pixels[i].green,
			frame->pixels[i].blue
		);
	}
	
	neo_reset();
}

int main(void)
{
	uart_init();
	LED_OUTPUT();
	
	static volatile frame_t frame = { 0 };
	frame.count = FRAME_LED_MAX;
	show_frame(&frame);
		
	while (1)
	{
		asm("nop");
		read_frame(&frame);
		asm("nop");
		show_frame(&frame);	
		
		uart_putchar(0xCC);
	}
}

