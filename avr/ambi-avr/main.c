// user libraries
#include "config.h"
#include "neo_apa104.h"

// platform libraries
#include <avr/io.h>
#include <avr/common.h>
#include <util/delay.h>

#define LED_COUNT 60

static void led_color(uint8_t r, uint8_t g, uint8_t b)
{
	for (uint8_t i = 0; i < LED_COUNT; ++i)
		neo_write_rgb(r, g, b);
	
	neo_reset();
}

int main(void)
{
	LED_OUTPUT();
		
	uint8_t r = 255;
	uint8_t g = 0;
	uint8_t b = 0;
	
	while (1)
	{
		for (uint8_t i = 0; i < 255; ++i)
		{
			++g;
			--r;
			
			led_color(r, g, b);
			_delay_ms(5);
		}
		
		for (uint8_t i = 0; i < 255; ++i)
		{
			++b;
			--g;
			
			led_color(r, g, b);
			_delay_ms(5);
		}
		
		for (uint8_t i = 0; i < 255; ++i)
		{
			++r;
			--b;
			
			led_color(r, g, b);
			_delay_ms(5);
		}
	}
}

