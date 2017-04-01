#pragma once

// std libraries
#include <inttypes.h>

// platform libraries
#include <avr/io.h>

// definitions and macros
#define F_CPU			16000000U

// LED Data pin
#define LED_DDR			DDRB
#define LED_PIN			PINB
#define LED_PORT		PORTB
#define LED_BIT			0
#define LED_MASK		(1 << LED_BIT)

#define LED_HIGH()		(LED_PORT |= LED_MASK)
#define LED_LOW()		(LED_PORT &= ~LED_MASK)
#define LED_TOGGLE()	(LED_PIN = LED_MASK)
#define LED_INPUT()		(LED_DDR &= ~LED_MASK)
#define LED_OUTPUT()	(LED_DDR |= LED_MASK)
