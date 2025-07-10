import { forwardRef } from "react";
import { ButtonProps } from "./buton-props";
import { buttonVariants } from "./button-variants";

const Button = forwardRef<HTMLButtonElement, ButtonProps>(
  ({ className, variant, size, textColor, children, radius, type, ...props }, ref) => {
    return (
      <button 
        className={buttonVariants({ variant, size, textColor, className, radius })} 
        ref={ref}
        type={type ? type : 'button'}
        {...props} 
      >
        {children}
      </button>
    );
  },
);
Button.displayName = 'Button'

export { Button };
