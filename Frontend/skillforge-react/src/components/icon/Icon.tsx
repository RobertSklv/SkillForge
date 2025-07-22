export interface IIconProps {
    type: string;
    classes?: string;
}

export function Icon (props: IIconProps) {
  return (
    <i className={`bi bi-${props.type} ${props.classes}`}></i>
  );
}
