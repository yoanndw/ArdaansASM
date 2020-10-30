<h1 style="font-size: 3em;">Ardaans Beta 0.1 Official Documentation</h1>

**DISCLAIMER: This documentation is the documentation for a incomplete Beta version. This reference might change in the future.**

Ardaans is made up of a virtual machine (VM), that interpret binary code (see [Opcodes](#opcodes)); and an assembler, which convert assembly code (see [Syntax](#assembly-language-syntax)) into binary, for the VM.

<!-- TODO: link to #Assembly Language Syntax -->

# VM characteristics
- 4 registers A, B, C, D : 1 byte each
- Program Memory, which stores the program in binary : 256 bytes
- Storage Memory (RAM) : 256 bytes

# Flags
There are only two flags on v0.1:
- Eq, for **Eq**uals
- Sm, for **Sm**aller

# Comparing
Comparing is done between one register and a *number*, or between two registers.

We can compare with the `cmp` instruction:
```x86asm
cmp <LHS> <RHS>
```
Where `<LHS>` is the left operand, and `<RHS>`, the right operand.

There are 3 cases:
- `<LHS>` < `<RHS>`: the `Sm` flag is set to *True*
- `<LHS>` = `<RHS>`: the `Eq` flag is set to *True*
- `<LHS>` >= `<RHS>`: both `Eq` and `Sm` are set to *False*

# Offset
The VM runs programs with a specific register: the *instruction pointer*. The instruction pointer indexes the current instruction byte in the Program Memory.

For example, if the assembly code is
```x86asm
mov a #$05
mov b #$0A

add a b
```

So, the binary code is
```
01 00 05
01 01 0A

08 00 01
```

The instruction pointer first points to address `$00` of the Program Memory, it executes the first `mov` instruction.
Then it points to `$03`. And finally, points to `$06`, for the `add` instruction.

These addresses are called *offsets*.

So:
- `mov a #$05` is at offset `$00`
- `mov b #$0A` is at offset `$03`
- `add a b` is at offset `$06`

Therefore, the previous machine code can be written
```
[00]   01 00 05
[03]   01 01 0A
[06]   08 00 01
```
to simplify. Where the value between the brackets is the offset of the instruction.

Now, if we want to go to a specified instruction, we just need to change the *instruction pointer* value. That's in what consists jumping.

For example, `jmp #$06` will *go to* offset `$06`, so it will execute (in the previous code) `add a b`.

# Jumping
In Ardaans v0.1, the only way to provide jumping is using *numbers*.
In fact, `jmp #$05` will jump to the *offset* `$05`.

There are also *conditional jumps*. These are specific jump instructions which jump according to flags values.

| Instruction |                          Explanation |
| ----------- | -----------------------------------: |
| `jeq`       |  Jumps if flag `Eq` is set to *True* |
| `jne`       | Jumps if flag `Eq` is set to *False* |
| `jsm`       |  Jumps if flag `Sm` is set to *True* |
| `jns`       | Jumps if flag `Sm` is set to *False* |

# Opcodes

## Registers
| Register | Binary Code |
| -------- | ----------: |
| A        |        `00` |
| B        |        `01` |
| C        |        `02` |
| D        |        `03` |

## Instructions
| Instruction  | Binary Code |                                                                                                              Explanation |
| ------------ | :---------: | -----------------------------------------------------------------------------------------------------------------------: |
| `mov a #$05` | `01 00 05`  |                                                                                 Puts the hex value `$05` in register `A` |
| `mov a &$05` | `02 00 05`  |                                                                      Puts the value at the address `$05` in register `A` |
| `mov &$05 a` | `03 05 00`  |                                                                      Puts the value of register `A` at the address `$05` |
| `mov a &b`   | `04 00 01`  |                                                 Puts the value at tha address contained in register `B`, in register `A` |
| `mov &a b`   | `05 00 01`  |                                                  Puts the value of register `B` at the address contained in register `A` |
| `mov a b`    | `06 00 01`  |                                                                           Puts the value of register `B` in register `A` |
| `add a #$05` | `07 00 05`  |                                                                  Adds `$05` to register `A`, then puts the result in `A` |
| `add a b`    | `08 00 01`  |                                                Adds the value stored in `B` to register `A`, then puts the result in `A` |
| `sub a #$05` | `09 00 05`  |                                                             Subtracts `$05` to register `A`, then puts the result in `A` |
| `sub a b`    | `0A 00 01`  |                                           Subtracts the value stored in `B` to register `A`, then puts the result in `A` |
| `mul a #$05` | `0B 00 05`  |                                                 Multiplies the value stored in `A` by `$05`, then puts the result in `A` |
| `mul a b`    | `0C 00 01`  |                               Multiplies the value stored in `A` by the value stored in `B`, then puts the result in `A` |
| `div a #$05` | `0D 00 05`  |                                                    Divides the value stored in `A` by `$05`, then puts the result in `A` |
| `div a b`    | `0E 00 01`  |                                  Divides the value stored in `A` by the value stored in `B`, then puts the result in `A` |
| `cmp a #$05` | `0F 00 05`  |                                                   Compares the value stored in `A` with `$05`. See [Compare](#comparing) |
| `cmp a b`    | `10 00 01`  |                                 Compares the value stored in `A` with the value stored in `B`. See [Compare](#comparing) |
| `inc a`      |   `11 00`   |                                                                                              Increments the register `A` |
| `dec b`      |   `12 00`   |                                                                                              Decrements the register `A` |
| `jmp #$05`   |   `13 05`   |                                                      Jumps to the byte `$05` of the Program Memory. See [Jump](#jumping) |
| `jeq #$05`   |   `14 05`   |  Jumps to offset `$05` of the Program Memory if the flag `Eq` is set to *True*. See [Flags](#flags) and [Jump](#jumping) |
| `jne #$05`   |   `15 05`   | Jumps to offset `$05` of the Program Memory if the flag `Eq` is set to *False*. See [Flags](#flags) and [Jump](#jumping) |
| `jsm #$05`   |   `16 05`   |  Jumps to offset `$05` of the Program Memory if the flag `Sm` is set to *True*. See [Flags](#flags) and [Jump](#jumping) |
| `jns #$05`   |   `17 05`   | Jumps to offset `$05` of the Program Memory if the flag `Sm` is set to *False*. See [Flags](#flags) and [Jump](#jumping) |

# Assembly Language Syntax

## General
An instrcution is an instruction keyword (`mov`, `add`, `sub`, ...), followed by one or two operands (register, number, ...), which are separated by spaces.

**Example**

`mov a b`: `mov` is the instruction keyword, and the operands are `a` and `b`.

**/!\ ATTENTION /!\\**: `mov a, b` is forbidden: operands **must** be separated by spaces.

## Number and Address
The only numerical value available is the hex value. It's represented by a `$`, followed by the hex value (e.g. `5` or `A`). This pattern (`$ + number`) will be called *numerical value* further in this documentation.

### Number
A *number* is a `#` followed by a *numerical value*: `#$5`, `#$A`

### Address
An *address* is a `&` followed by:
- A *numerical value*. So it points to the address <*numerical value*>. **Example:** `&$5` points to the address `$5`.
- A *register*. So it points to the address stored in <*register*>. **Example:** if `A` equals `$5`, `&a` points to the address `$5`.
