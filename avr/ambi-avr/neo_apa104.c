// user libraries
#include "neo_apa104.h"
#include "config.h"

// platform libraries
#include <util/delay.h>

// definitions and macros
#define NS_TO_CYCLES(x) ( x / (1000000000L / F_CPU) )

#define T1H  900    // Width of a 1 bit in ns
#define T1L  600    // Width of a 1 bit in ns
#define T0H  400    // Width of a 0 bit in ns
#define T0L  900    // Width of a 0 bit in ns
#define RES 6000    // Width of the low gap between bits to cause a frame to latch

// declarations
static inline void write_bit(uint8_t val);
static void write_byte(uint8_t byte);

void neo_write_rgb(uint8_t r, uint8_t g, uint8_t b)
{
	write_byte(g);
	write_byte(r);
	write_byte(b);
}

void neo_reset()
{
	_delay_us((RES / 1000UL) + 1);
}

static inline void write_bit(uint8_t val)
{
	if (val)
	{
		asm volatile (
		"sbi %[port], %[bit] \n\t"
		".rept %[onCycles] \n\t"
		"nop \n\t"
		".endr \n\t"
		"cbi %[port], %[bit] \n\t"
		".rept %[offCycles] \n\t"
		"nop \n\t"
		".endr \n\t"
		::
		[port]		"I" (_SFR_IO_ADDR(LED_PORT)),
		[bit]		"I" (LED_BIT),
		[onCycles]	"I" (NS_TO_CYCLES(T1H) - 2),
		[offCycles]	"I" (NS_TO_CYCLES(T1L) - 2)
		);
	}
	else
	{
		asm volatile (
		"sbi %[port], %[bit] \n\t"
		".rept %[onCycles] \n\t"
		"nop \n\t"
		".endr \n\t"
		"cbi %[port], %[bit] \n\t"
		".rept %[offCycles] \n\t"
		"nop \n\t"
		".endr \n\t"
		::
		[port]		"I" (_SFR_IO_ADDR(LED_PORT)),
		[bit]		"I" (LED_BIT),
		[onCycles]	"I" (NS_TO_CYCLES(T0H) - 2),
		[offCycles]	"I" (NS_TO_CYCLES(T0L) - 2)
		);
	}
}

static void write_byte(uint8_t byte)
{
	for (int8_t i = 7; i >= 0; --i)
		write_bit(byte & (1 << i));
}
